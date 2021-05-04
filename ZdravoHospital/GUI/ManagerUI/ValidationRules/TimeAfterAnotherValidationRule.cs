using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;

using Model;

namespace ZdravoHospital.GUI.ManagerUI.ValidationRules
{
    public class TimeAfterAnotherValidationRule : ValidationRule
    {
        public OtherPassedTimeWrapper Wrapper { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var input = value.ToString().Trim();

            if (input.Equals(string.Empty))
                return new ValidationResult(false, "'Time' cannot be left empty...");

            if (input.IndexOf("-") != -1 || input.IndexOf(".") != -1 || input.IndexOf(":") == -1)
                return new ValidationResult(false, "The correct format is 'HH:mm(:ss)' \n eg 19:45:13");

            try
            {
                var timeOfDay = TimeSpan.ParseExact(input, "c", null);
                var endDate = Wrapper.ThisDate.Add(timeOfDay);

                var otherTimeOfDay = TimeSpan.ParseExact(Wrapper.OtherPassedTime, "c", null);
                var startDate = Wrapper.OtherPassedDate.Add(otherTimeOfDay);
                if (startDate >= endDate)
                {
                    return new ValidationResult(false, "'Start time' is ahead of 'End time'...");
                }

                var answer = CheckIntersectPeriods(startDate, endDate, Wrapper.PassedRoom);
                if (!answer.Equals(string.Empty))
                {
                    return new ValidationResult(false, answer);
                }

                answer = CheckIntersectRoomSchedule(startDate, endDate, Wrapper.PassedRoom);
                if (!answer.Equals(string.Empty))
                {
                    return new ValidationResult(false, answer);
                }

            }
            catch
            {
                return new ValidationResult(false, null);
            }

            return new ValidationResult(true, null);
        }

        public string CheckIntersectPeriods(DateTime start, DateTime end, Room room)
        {
            foreach (var p in Model.Resources.periods)
            {
                if (room.Id == p.RoomId)
                {
                    var endTime = p.StartTime.AddMinutes(p.Duration);
                    if ((start >= p.StartTime && endTime >= start) ||
                        (p.StartTime >= start && end >= endTime) ||
                        (end >= p.StartTime && endTime >= end))
                    {
                        /* Intersects with a period */
                        var startTime = p.StartTime;
                        var sb = new StringBuilder();
                        sb.Append("The time span you entered intersects with a medical intervention that is scheduled for ");
                        sb.Append(startTime.Day).Append("/").Append(startTime.Month).Append("/").Append(startTime.Year);
                        sb.Append(", lasting from ").Append(startTime.Hour).Append(":").Append(startTime.Minute).Append(" until ");
                        sb.Append(endTime.Hour).Append(":").Append(endTime.Minute);

                        return sb.ToString();
                    }
                }
            }

            return string.Empty;
        }

        public string CheckIntersectRoomSchedule(DateTime start, DateTime end, Room room)
        {
            foreach (var rs in Model.Resources.roomSchedule)
            {
                if (rs.ScheduleType == ReservationType.TRANSFER)
                    continue;
                if (room.Id == rs.RoomId)
                {
                    if ((start >= rs.StartTime && rs.EndTime >= start) ||
                        (rs.StartTime >= start && end >= rs.EndTime) ||
                        (end >= rs.StartTime && rs.EndTime >= end))
                    {
                        /* Intersects with a period */
                        var startTime = rs.StartTime;
                        var endTime = rs.EndTime;
                        var sb = new StringBuilder();
                        sb.Append("The time span you entered intersects with room renovation that is scheduled for ");
                        sb.Append(startTime.Day).Append("/").Append(startTime.Month).Append("/").Append(startTime.Year);
                        sb.Append(", ").Append(startTime.Hour).Append(":").Append(startTime.Minute).Append(" until ");
                        sb.Append(endTime.Day).Append("/").Append(endTime.Month).Append("/").Append(endTime.Year).Append(", ");
                        sb.Append(endTime.Hour).Append(":").Append(endTime.Minute);

                        return sb.ToString();
                    }
                }
            }

            return string.Empty;
        }
    }

    public class OtherPassedTimeWrapper : DependencyObject
    {
        public static readonly DependencyProperty OtherPassedDateProperty = DependencyProperty.Register("OtherPassedDate", typeof(DateTime), typeof(OtherPassedTimeWrapper), null);
    
        public DateTime OtherPassedDate
        {
            get => (DateTime)GetValue(OtherPassedDateProperty);
            set => SetValue(OtherPassedDateProperty, value);
        }

        public DateTime ThisDate
        {
            get => (DateTime)GetValue(ThisDateProperty);
            set => SetValue(ThisDateProperty, value);
        }

        public static readonly DependencyProperty ThisDateProperty = DependencyProperty.Register("ThisDate", typeof(DateTime), typeof(OtherPassedTimeWrapper), null);


        public string OtherPassedTime
        {
            get => (string)GetValue(OtherPassedTimeProperty);
            set => SetValue(OtherPassedTimeProperty, value);
        }
        
        public static readonly DependencyProperty OtherPassedTimeProperty = DependencyProperty.Register("OtherPassedTime", typeof(string), typeof(OtherPassedTimeWrapper), null);

        public static readonly DependencyProperty PassedRoomProperty = DependencyProperty.Register("PassedRoom", typeof(Room), typeof(OtherPassedTimeWrapper), null);

        public Room PassedRoom
        {
            get => (Room)GetValue(PassedRoomProperty);
            set => SetValue(PassedRoomProperty, value);
        }
    }

    public class OtherPassedTimeBindingProxy : Freezable
    {
        protected override Freezable CreateInstanceCore()
        {
            return new OtherPassedTimeBindingProxy();
        }

        public object Data
        {
            get => (object)GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }

        public static readonly DependencyProperty DataProperty = DependencyProperty.Register("Data", typeof(object), typeof(OtherPassedTimeBindingProxy), new PropertyMetadata(null));
    }
}
