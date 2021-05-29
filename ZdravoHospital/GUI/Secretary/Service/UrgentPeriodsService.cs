using Model;
using Repository.DoctorPersistance;
using Repository.PatientPersistance;
using Repository.PeriodPersistance;
using Repository.RoomPersistance;
using Repository.SpecializationPersistance;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using ZdravoHospital.GUI.Secretary.DTOs;

namespace ZdravoHospital.GUI.Secretary.Service
{
    public class UrgentPeriodsService
    {
        private IPeriodRepository _periodRepository;
        private ISpecializationRepository _specializationRepository;
        private IPatientRepository _patientRepository;
        private IDoctorRepository _doctorRepository;
        private IRoomRepository _roomRepository;

        public UrgentPeriodsService()
        {
            _periodRepository = new PeriodRepository();
            _specializationRepository = new SpecializationRepository();
            _patientRepository = new PatientRepository();
            _doctorRepository = new DoctorRepository();
            _roomRepository = new RoomRepository();
        }

        public List<Period> GetPeriods()
        {
            return _periodRepository.GetValues();
        }
        public List<Specialization> GetSpecializations()
        {
            return _specializationRepository.GetValues();
        }
        public List<Patient> GetPatients()
        {
            return _patientRepository.GetValues();
        }
        public List<Doctor> GetDoctors()
        {
            return _doctorRepository.GetValues();
        }

        public List<Period> findFreePeriods(UrgentPeriodDTO urgentPeriodDTO)
        {
            List<Doctor> doctors = findDoctorsBySpecialization(urgentPeriodDTO.SelectedSpecialization);

            List<Period> freePeriods = new List<Period>();
            int duration = Int32.Parse(urgentPeriodDTO.Duration);

            foreach (Doctor doctor in doctors)
            {
                DateTime startCheckTime = DateTime.Now;
                DateTime endCheckTime = startCheckTime.AddMinutes(60);
                while (startCheckTime < endCheckTime)
                {
                    Period tryPeriod = new Period(startCheckTime, duration, urgentPeriodDTO.Patient.Username, doctor.Username, true);
                    if (this.IsPeriodAvailable(tryPeriod))
                    {
                        freePeriods.Add(tryPeriod);
                        break;
                    }
                    startCheckTime = startCheckTime.AddMinutes(1);
                }
            }

            return freePeriods;
        }

        private List<Doctor> findDoctorsBySpecialization(Specialization specialization)
        {
            List<Doctor> doctors = new List<Doctor>();
            List<Doctor> allDoctors = GetDoctors();
            foreach (var doctor in allDoctors)
            {
                if (doctor.SpecialistType.SpecializationName.Equals(specialization.SpecializationName))
                {
                    doctors.Add(doctor);
                }
            }
            return doctors;
        }

        private bool IsPeriodAvailable(Period period)
        {
            DateTime periodEndtime = period.StartTime.AddMinutes(period.Duration);
            List<Period> periods = GetPeriods();

            foreach (Period existingPeriod in periods)
            {
                DateTime existingPeriodEndTime = existingPeriod.StartTime.AddMinutes(existingPeriod.Duration);

                if (period.DoctorUsername == existingPeriod.DoctorUsername)
                {
                    if (period.StartTime < existingPeriodEndTime && periodEndtime > existingPeriod.StartTime)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private Period findBestPeriod(List<Period> periods)
        {
            Period bestPeriod = periods[0];
            foreach (var period in periods)
            {
                if (period.StartTime < bestPeriod.StartTime)
                {
                    bestPeriod = period;
                }
            }
            return bestPeriod;
        }

        private List<Period> findPeriodsWithinRange(List<Period> periods, DateTime start, int duration)
        {
            List<Period> periodsWithinRange = new List<Period>();
            DateTime existingPeriodEndTime = start.AddMinutes(duration);
            foreach (var period in periods)
            {
                DateTime periodEndtime = period.StartTime.AddMinutes(period.Duration);
                if (period.StartTime < existingPeriodEndTime && periodEndtime > start)
                {
                    periodsWithinRange.Add(period);
                }
            }
            return periodsWithinRange;
        }

        private List<Period> findPeriodsByDoctor(string doctorUsername)
        {
            List<Period> doctorPeriods = new List<Period>();
            List<Period> allPeriods = GetPeriods();
            foreach (var period in allPeriods)
            {
                if (period.DoctorUsername == doctorUsername)
                {
                    doctorPeriods.Add(period);
                }
            }
            return doctorPeriods;
        }

        private List<Period> findTryoutPeriods(List<Period> periodsWithinRange, UrgentPeriodDTO urgentPeriodDTO)
        {
            // creating periods now and on the end of every existing period within range [0, 1h + duration] if point is within [0, +1h]
            List<DateTime> timePoints = new List<DateTime>();
            List<Period> tryoutPeriods = new List<Period>();
            string doctorUsername = periodsWithinRange[0].DoctorUsername;
            if (!isTimePointWithinAnyPeriod(DateTime.Now, periodsWithinRange))
                timePoints.Add(DateTime.Now);
            foreach (var period in periodsWithinRange)
            {
                if (period.StartTime.AddMinutes(period.Duration) < DateTime.Now.AddMinutes(60))
                {
                    timePoints.Add(period.StartTime.AddMinutes(period.Duration));
                }
            }
            foreach (var tp in timePoints)
            {
                Period tryPeriod = new Period(tp, Int32.Parse(urgentPeriodDTO.Duration), urgentPeriodDTO.Patient.Username, doctorUsername, true);
                //check if tryperiod overlaps urgent period
                List<Period> urgentPeriods = findUrgentPeriods(periodsWithinRange);
                int periodsOverlappingUrgent = findPeriodsWithinRange(urgentPeriods, tryPeriod.StartTime, tryPeriod.Duration).Count;
                if (periodsOverlappingUrgent == 0)
                    tryoutPeriods.Add(tryPeriod);
            }
            return tryoutPeriods;
        }

        private List<Period> findUrgentPeriods(List<Period> periods)
        {
            List<Period> urgentPeriods = new List<Period>();
            foreach (var period in periods)
            {
                if (period.IsUrgent)
                    urgentPeriods.Add(period);
            }
            return urgentPeriods;
        }

        private bool isTimePointWithinPeriod(DateTime point, Period period)
        {
            if (point > period.StartTime && point < period.StartTime.AddMinutes(period.Duration))
                return true;
            else
                return false;
        }

        private bool isTimePointWithinAnyPeriod(DateTime point, List<Period> periods)
        {
            foreach (Period period in periods)
            {
                if (isTimePointWithinPeriod(point, period))
                {
                    return true;
                }
            }
            return false;
        }

        private void setMovePeriods(List<Period> tryoutPeriods, List<Period> periodsWithinRange)
        {
            foreach (var tryPeriod in tryoutPeriods)
            {
                List<Period> overlappingPeriods = findPeriodsWithinRange(periodsWithinRange, tryPeriod.StartTime, tryPeriod.Duration);
                overlappingPeriods.Sort((x, y) => x.StartTime.CompareTo(y.StartTime));
                foreach (var overlap in overlappingPeriods)
                {
                    DateTime initialStartTime = overlap.StartTime;
                    DateTime newStartTime = findFreeStartTime(overlap, tryPeriod.MovePeriods, tryPeriod.StartTime.AddMinutes(tryPeriod.Duration), overlappingPeriods);
                    MovePeriod movePeriod = new MovePeriod(overlap.DoctorUsername, overlap.PatientUsername, overlap.RoomId, initialStartTime, newStartTime, overlap.Duration);
                    tryPeriod.MovePeriods.Add(movePeriod);
                }
            }
        }

        private DateTime findFreeStartTime(Period period, ObservableCollection<MovePeriod> movePeriods, DateTime urgentEndTime, List<Period> overlappingPeriods)
        {
            List<Period> overlappingPeriodsFinal = this.cloneListOfPeriods(overlappingPeriods);
            DateTime initialStartTime = period.StartTime;
            period.StartTime = urgentEndTime;

            while (!IsPeriodAvailableIncludingMovePeriods(period, movePeriods, overlappingPeriodsFinal))
            {
                period.StartTime = period.StartTime.AddMinutes(1);
            }
            DateTime newStartTime = period.StartTime;
            period.StartTime = initialStartTime;

            return newStartTime;
        }

        private bool IsPeriodAvailableIncludingMovePeriods(Period period, ObservableCollection<MovePeriod> movePeriods, List<Period> overlappingPeriods)
        {

            DateTime periodEndtime = period.StartTime.AddMinutes(period.Duration);
            List<Period> periods = GetPeriods();

            foreach (Period existingPeriod in periods)
            {
                bool isOverlaping = false;
                foreach (var overlap in overlappingPeriods)
                {
                    if (existingPeriod.StartTime == overlap.StartTime && existingPeriod.RoomId == overlap.RoomId)
                        isOverlaping = true;
                }

                if (isOverlaping)
                    continue;

                DateTime existingPeriodEndTime = existingPeriod.StartTime.AddMinutes(existingPeriod.Duration);

                if (period.DoctorUsername == existingPeriod.DoctorUsername)
                {
                    if (period.StartTime < existingPeriodEndTime && periodEndtime > existingPeriod.StartTime)
                    {
                        return false;
                    }
                }
            }

            foreach (MovePeriod mp in movePeriods)
            {
                DateTime existingMovePeriodEndTime = mp.MovedStartTime.AddMinutes(mp.Duration);
                if (period.StartTime < existingMovePeriodEndTime && periodEndtime > mp.MovedStartTime)
                {
                    return false;
                }
            }

            return true;
        }

        private List<Period> cloneListOfPeriods(List<Period> periods)
        {
            List<Period> newList = new List<Period>();
            foreach (var period in periods)
            {
                Period newPeriod = new Period();
                newPeriod.StartTime = period.StartTime;
                newPeriod.RoomId = period.RoomId;
                newPeriod.Duration = period.Duration;
                newList.Add(newPeriod);
            }
            return newList;
        }
        public List<Room> GetRooms()
        {
            return _roomRepository.GetValues();
        }

        private List<Room> findAvailableEmergencyRooms(Period newPeriod)
        {
            List<Room> allRooms = GetRooms();
            List<Room> availableRooms = new List<Room>();
            foreach (var room in allRooms)
            {
                if (room.RoomType == RoomType.EMERGENCY_ROOM)
                {
                    bool available = true;
                    List<Period> periods = GetPeriods();
                    foreach (Period existingPeriod in periods)
                    {
                        if (periodsOverlap(newPeriod, existingPeriod))
                        {
                            if (room.Id == existingPeriod.RoomId)
                                available = false;
                        }
                    }
                    if (available)
                        availableRooms.Add(room);
                }

            }
            return availableRooms;
        }

        private bool periodsOverlap(Period newPeriod, Period existingPeriod)
        {
            DateTime existingPeriodEndTime = existingPeriod.StartTime.AddMinutes(existingPeriod.Duration);
            DateTime newPeriodEndtime = newPeriod.StartTime.AddMinutes(newPeriod.Duration);
            if (newPeriod.StartTime < existingPeriodEndTime && newPeriodEndtime > existingPeriod.StartTime)
            {
                return true;
            }
            return false;
        }

        public PeriodsViewHolderDTO ProcessUrgentPeriodCreation(UrgentPeriodDTO urgentPeriodDTO)
        {
            PeriodsViewHolderDTO viewHolder = new PeriodsViewHolderDTO();
            List<Period> freePeriods = findFreePeriods(urgentPeriodDTO);
            if (freePeriods.Count == 0)
            {
                //Logika za kad nema slobodnih termina
                List<Doctor> doctors = findDoctorsBySpecialization(urgentPeriodDTO.SelectedSpecialization);
                List<Period> periods = new List<Period>();

                foreach (var doctor in doctors)
                {
                    List<Period> doctorPeriods = findPeriodsByDoctor(doctor.Username);
                    List<Period> periodsWithinRange = findPeriodsWithinRange(doctorPeriods, DateTime.Now, 60 + Int32.Parse(urgentPeriodDTO.Duration));
                    List<Period> tryoutPeriods = findTryoutPeriods(periodsWithinRange, urgentPeriodDTO);
                    setMovePeriods(tryoutPeriods, periodsWithinRange);
                    foreach (var tryout in tryoutPeriods)
                    {
                        if (tryout.MovePeriods.Count != 0)
                            periods.Add(tryout);
                    }
                }

                if (periods.Count == 0)
                {
                    viewHolder.Status = UrgentPeriodStatus.NO_DOCTORS_AVAILABLE;
                    return viewHolder;
                }
                else
                {
                    viewHolder.Status = UrgentPeriodStatus.PERIODS_TO_MOVE;
                    viewHolder.Periods = periods;
                    return viewHolder;
                }
                    
            }
            else
            {
                Period bestPeriod = findBestPeriod(freePeriods);

                //set room
                List<Room> availableEmergencyRooms = findAvailableEmergencyRooms(bestPeriod);
                if (availableEmergencyRooms.Count != 0)
                    bestPeriod.RoomId = availableEmergencyRooms[0].Id;

                _periodRepository.Create(bestPeriod);
                viewHolder.Status = UrgentPeriodStatus.FREE_TO_BOOK;
                viewHolder.BestPeriod = bestPeriod;
                return viewHolder;
            }
        }
    }
}
