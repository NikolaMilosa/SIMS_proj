using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;

using Model;
using Repository.PeriodPersistance;
using Repository.RoomSchedulePersistance;

namespace ZdravoHospital.GUI.ManagerUI.ValidationRules
{
    class TimeInputValidationRule : ValidationRule
    {

        public PassedTimeWrapper Wrapper { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var input = value.ToString().Trim();

            if (input.Equals(string.Empty))
                return new ValidationResult(false, "'Time' cannot be left empty...");

            if(input.IndexOf("-") != -1 || input.IndexOf(".") != -1 || input.IndexOf(":") == -1)
                return new ValidationResult(false, "The correct format is 'HH:mm(:ss)' \n eg 19:45:13");

            try
            {
                var timeOfDay = Wrapper.PassedTime.Add(TimeSpan.ParseExact(input, "c", null));
                if (timeOfDay <= DateTime.Now)
                {
                    return new ValidationResult(false, "Time you typed has already passed...");
                }

                if (Wrapper.PassedFirstRoom.Id == Wrapper.PassedSecondRoom.Id)
                {
                    /* Checking for renovation */
                    var answer = CheckIntersectPeriods(timeOfDay,Wrapper.PassedFirstRoom, Wrapper.PassedSecondRoom);
                    if (!answer.Equals(string.Empty))
                        return new ValidationResult(false, "There is a medical intervention planned at that time... " + answer);

                    answer = CheckIntersectRenovations(timeOfDay, Wrapper.PassedFirstRoom, Wrapper.PassedSecondRoom);
                    if (!answer.Equals(string.Empty))
                        return new ValidationResult(false, "There is a renovation already planned at that time... " + answer);
                }
                else
                {
                    /* Checking for inventory transport */
                    var answer = CheckIntersectPeriods(timeOfDay, Wrapper.PassedFirstRoom, Wrapper.PassedSecondRoom);
                    if (!answer.Equals(string.Empty))
                        return new ValidationResult(false, "There is a medical intervention planned at that time..." + answer);
                }

            }
            catch
            {
                return new ValidationResult(false, "The correct format is 'HH:mm(:ss)' \n eg 19:45:13");
            }

            return new ValidationResult(true, null);
        }

        public string CheckIntersectPeriods(DateTime passedTime, Room firstRoom, Room secondRoom)
        {
            var periodRepository = new PeriodRepository();

            foreach (var p in periodRepository.GetValues())
            {
                var endTime = p.StartTime.AddMinutes(p.Duration);
                if (passedTime >= p.StartTime && passedTime <= endTime && firstRoom.Id == p.RoomId)
                {
                    /* StartTime is in the middle of a period */
                    var sb = new StringBuilder();
                    sb.Append("Room with id '").Append(firstRoom.Id).Append("' is busy and will be available from ");
                    sb.Append(endTime.Day).Append("/").Append(endTime.Month).Append("/").Append(endTime.Year);
                    sb.Append(" at ").Append(endTime.Hour).Append(":").Append(endTime.Minute);
                    return sb.ToString();
                }
                else if (passedTime >= p.StartTime && passedTime <= endTime && secondRoom.Id == p.RoomId)
                {
                    /* StartTime is in the middle of a period */
                    var sb = new StringBuilder();
                    sb.Append("Room with id '").Append(secondRoom.Id).Append("' is busy and will be available from ");
                    sb.Append(endTime.Day).Append("/").Append(endTime.Month).Append("/").Append(endTime.Year);
                    sb.Append(" at ").Append(endTime.Hour).Append(":").Append(endTime.Minute);
                    return sb.ToString();
                }
            }

            return string.Empty;
        }

        public string CheckIntersectRenovations(DateTime passedTime, Room firstRoom, Room secondRoom)
        {
            var roomScheduleRepository = new RoomScheduleRepository();
            foreach (var r in roomScheduleRepository.GetValues())
            {
                if (r.ScheduleType == ReservationType.TRANSFER)
                    continue;
                if (passedTime >= r.StartTime && passedTime <= r.EndTime && r.RoomId == firstRoom.Id)
                {
                    /* Start time is in the middle of another renovation */
                    var endTime = r.EndTime;
                    var sb = new StringBuilder();
                    sb.Append("Room with id '").Append(firstRoom.Id).Append("' is busy and will be available from ");
                    sb.Append(endTime.Day).Append("/").Append(endTime.Month).Append("/").Append(endTime.Year);
                    sb.Append(" at ").Append(endTime.Hour).Append(":").Append(endTime.Minute);
                    return sb.ToString();
                }
                else if (passedTime >= r.StartTime && passedTime <= r.EndTime && r.RoomId == secondRoom.Id)
                {
                    /* Start time is in the middle of another renovation */
                    var endTime = r.EndTime;
                    var sb = new StringBuilder();
                    sb.Append("Room with id '").Append(secondRoom.Id).Append("' is busy and will be available from ");
                    sb.Append(endTime.Day).Append("/").Append(endTime.Month).Append("/").Append(endTime.Year);
                    sb.Append(" at ").Append(endTime.Hour).Append(":").Append(endTime.Minute);
                    return sb.ToString();
                }
            }

            return string.Empty;
        }
    }

    class PassedTimeWrapper : DependencyObject
    {
        public static readonly DependencyProperty PassedTimeProperty = DependencyProperty.Register("PassedTime", typeof(DateTime), typeof(PassedTimeWrapper), null);
        
        public DateTime PassedTime
        {
            get => (DateTime)GetValue(PassedTimeProperty);
            set => SetValue(PassedTimeProperty, value);
        }
        
        public static readonly DependencyProperty PassedFirstRoomProperty = DependencyProperty.Register("PassedFirstRoom", typeof(Room), typeof(PassedTimeWrapper), null);

        public Room PassedFirstRoom
        {
            get => (Room)GetValue(PassedFirstRoomProperty);
            set => SetValue(PassedFirstRoomProperty, value);
        }

        public static readonly DependencyProperty PassedSecondRoomProperty = DependencyProperty.Register("PassedSecondRoom", typeof(Room), typeof(PassedTimeWrapper), null);

        public Room PassedSecondRoom
        {
            get => (Room)GetValue(PassedSecondRoomProperty);
            set => SetValue(PassedSecondRoomProperty, value);
        }
    }

    public class PassedTimeBindingProxy : Freezable
    {
        protected override Freezable CreateInstanceCore()
        {
            return new PassedTimeBindingProxy();
        }

        public object Data
        {
            get => (object)GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }

        public static readonly DependencyProperty DataProperty = DependencyProperty.Register("Data", typeof(object), typeof(PassedTimeBindingProxy), new PropertyMetadata(null));
    }
}
