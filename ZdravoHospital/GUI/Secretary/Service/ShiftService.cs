using Model;
using Repository.DoctorPersistance;
using System;
using System.Collections.Generic;
using System.Text;
using ZdravoHospital.GUI.Secretary.DTOs;

namespace ZdravoHospital.GUI.Secretary.Service
{
    public class ShiftService
    {
        private IDoctorRepository _doctorRepository;
        public ShiftService()
        {
            _doctorRepository = new DoctorRepository();
        }

        public void ProcessShiftCreation(Doctor selectedDoctor, ShiftDTO shiftDTO)
        {
            if (shiftDTO.IsSingleDayShift)
                saveSingleDayShift(selectedDoctor, shiftDTO);
            else
                saveRegularShift(selectedDoctor, shiftDTO);
        }

        private void saveSingleDayShift(Doctor selectedDoctor, ShiftDTO shiftDTO)
        {
            //selectedDoctor.ShiftRule.SingleDayShifts.Add(new DoctorsShift(shiftDTO.ScheduledShift, shiftDTO.ShiftStart, shiftDTO.IsSingleDayShift));
            //_doctorRepository.Update(selectedDoctor);
            addSingleDayShift(selectedDoctor, shiftDTO);
        }

        private void addSingleDayShift(Doctor selectedDoctor, ShiftDTO shiftDTO)
        {
            foreach(var shift in selectedDoctor.ShiftRule.SingleDayShifts)
            {
                if(shift.ShiftStart.Date == shiftDTO.ShiftStart.Date)
                {
                    shift.ScheduledShift = shiftDTO.ScheduledShift;
                    _doctorRepository.Update(selectedDoctor);
                    return;
                }
            }
            selectedDoctor.ShiftRule.SingleDayShifts.Add(new DoctorsShift(shiftDTO.ScheduledShift, shiftDTO.ShiftStart, shiftDTO.IsSingleDayShift));
            _doctorRepository.Update(selectedDoctor);
        }

        private void saveRegularShift(Doctor selectedDoctor, ShiftDTO shiftDTO)
        {
            selectedDoctor.ShiftRule.RegularShift = new DoctorsShift(shiftDTO.ScheduledShift, shiftDTO.ShiftStart, shiftDTO.IsSingleDayShift);
            _doctorRepository.Update(selectedDoctor);
        }
    }
}
