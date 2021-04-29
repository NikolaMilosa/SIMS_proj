using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;

using Model;

namespace ZdravoHospital.GUI.ManagerUI.ValidationRules
{
    class TimeInputValidationRule : ValidationRule
    {

        public PassedTimeWrapper Wrapper { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string input = value.ToString().Trim();

            if (input.Equals(String.Empty))
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
                    string answer = CheckIntersectPeriods(timeOfDay,Wrapper.PassedFirstRoom, Wrapper.PassedSecondRoom);
                    if (!answer.Equals(String.Empty))
                        return new ValidationResult(false, "There is a medical intervention planned at that time... " + answer);

                    answer = CheckIntersectRenovations(timeOfDay, Wrapper.PassedFirstRoom, Wrapper.PassedSecondRoom);
                    if (!answer.Equals(String.Empty))
                        return new ValidationResult(false, "There is a renovation already planned at that time... " + answer);
                }
                else
                {
                    /* Checking for inventory transport */
                    string answer = CheckIntersectPeriods(timeOfDay, Wrapper.PassedFirstRoom, Wrapper.PassedSecondRoom);
                    if (!answer.Equals(String.Empty))
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
            foreach (Period p in Model.Resources.periods)
            {
                DateTime endTime = p.StartTime.AddMinutes(p.Duration);
                if (passedTime >= p.StartTime && passedTime <= endTime && firstRoom.Id == p.RoomId)
                {
                    /* StartTime is in the middle of a period */
                    StringBuilder sb = new StringBuilder();
                    sb.Append("Room with id '").Append(firstRoom.Id).Append("' is busy and will be available from ");
                    sb.Append(endTime.Day).Append("/").Append(endTime.Month).Append("/").Append(endTime.Year);
                    sb.Append(" at ").Append(endTime.Hour).Append(":").Append(endTime.Minute);
                    return sb.ToString();
                }
                else if (passedTime >= p.StartTime && passedTime <= endTime && secondRoom.Id == p.RoomId)
                {
                    /* StartTime is in the middle of a period */
                    StringBuilder sb = new StringBuilder();
                    sb.Append("Room with id '").Append(secondRoom.Id).Append("' is busy and will be available from ");
                    sb.Append(endTime.Day).Append("/").Append(endTime.Month).Append("/").Append(endTime.Year);
                    sb.Append(" at ").Append(endTime.Hour).Append(":").Append(endTime.Minute);
                    return sb.ToString();
                }
            }

            return String.Empty;
        }

        public string CheckIntersectRenovations(DateTime passedTime, Room firstRoom, Room secondRoom)
        {
            foreach (RoomSchedule r in Model.Resources.roomSchedule)
            {
                if (r.ScheduleType == ReservationType.TRANSFER)
                    continue;
                if (passedTime >= r.StartTime && passedTime <= r.EndTime && r.RoomId == firstRoom.Id)
                {
                    /* Start time is in the middle of another renovation */
                    DateTime endTime = r.EndTime;
                    StringBuilder sb = new StringBuilder();
                    sb.Append("Room with id '").Append(firstRoom.Id).Append("' is busy and will be available from ");
                    sb.Append(endTime.Day).Append("/").Append(endTime.Month).Append("/").Append(endTime.Year);
                    sb.Append(" at ").Append(endTime.Hour).Append(":").Append(endTime.Minute);
                    return sb.ToString();
                }
                else if (passedTime >= r.StartTime && passedTime <= r.EndTime && r.RoomId == secondRoom.Id)
                {
                    /* Start time is in the middle of another renovation */
                    DateTime endTime = r.EndTime;
                    StringBuilder sb = new StringBuilder();
                    sb.Append("Room with id '").Append(secondRoom.Id).Append("' is busy and will be available from ");
                    sb.Append(endTime.Day).Append("/").Append(endTime.Month).Append("/").Append(endTime.Year);
                    sb.Append(" at ").Append(endTime.Hour).Append(":").Append(endTime.Minute);
                    return sb.ToString();
                }
            }

            return String.Empty;
        }
    }

    class PassedTimeWrapper : DependencyObject
    {
        public static readonly DependencyProperty PassedTimeProeprty = DependencyProperty.Register("PassedTime", typeof(DateTime), typeof(PassedTimeWrapper), null);
        
        public DateTime PassedTime
        {
            get { return (DateTime)GetValue(PassedTimeProeprty); }
            set { SetValue(PassedTimeProeprty, value); }
        }
        
        public static readonly DependencyProperty PassedFirstRoomProperty = DependencyProperty.Register("PassedFirstRoom", typeof(Room), typeof(PassedTimeWrapper), null);

        public Room PassedFirstRoom
        {
            get { return (Room)GetValue(PassedFirstRoomProperty); }
            set { SetValue(PassedFirstRoomProperty, value); }
        }

        public static readonly DependencyProperty PassedSecondRoomProperty = DependencyProperty.Register("PassedSecondRoom", typeof(Room), typeof(PassedTimeWrapper), null);

        public Room PassedSecondRoom
        {
            get { return (Room)GetValue(PassedSecondRoomProperty); }
            set { SetValue(PassedSecondRoomProperty, value); }
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
            get { return (object)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        public static readonly DependencyProperty DataProperty = DependencyProperty.Register("Data", typeof(object), typeof(PassedTimeBindingProxy), new PropertyMetadata(null));
    }
}
