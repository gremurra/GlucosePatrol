﻿using GlucosePatrol.Data;
using GlucosePatrol.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlucosePatrol.Services
{
    public class PatientService
    {
        private readonly Guid _userId;
        public PatientService(Guid userId)
        {
            _userId = userId;
        }

        public bool CreatePatient(PatientCreate model)
        {
            var entity =
                new Patient()
                {
                    OwnerId = _userId,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    DateOfBirth = model.DateOfBirth,
                    WeightInPounds = model.WeightInPounds,
                    HeightInInches = model.HeightInInches,
                    Gender = model.Gender
                };
            using (var ctx = new ApplicationDbContext())
            {
                ctx.Patients.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        public IEnumerable<PatientListItem> GetPatients()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                    .Patients
                    .Where(e => e.OwnerId == _userId)
                    .Select(
                        e =>
                        new PatientListItem
                        {
                            PatientId = e.PatientId,
                            FirstName = e.FirstName,
                            LastName = e.LastName,
                            DateOfBirth = e.DateOfBirth,
                            WeightInPounds = e.WeightInPounds,
                            HeightInInches = e.HeightInInches,
                            Gender = e.Gender,
                        }
                        );
                return query.ToArray();
            }
        }

        public bool UpdatePatient(PatientEdit model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                    .Patients
                    .Single(e => e.PatientId == model.PatientId && e.OwnerId == _userId);
                entity.FirstName = model.FirstName;
                entity.LastName = model.LastName;
                entity.DateOfBirth = model.DateOfBirth;
                entity.WeightInPounds = model.WeightInPounds;
                entity.HeightInInches = model.HeightInInches;
                entity.Gender = model.Gender;
                return ctx.SaveChanges() == 1;
            }
        }

        public bool DeletePatient(int patientId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                    .Patients
                    .Single(e => e.PatientId == patientId && e.OwnerId == _userId);
                ctx.Patients.Remove(entity);
                return ctx.SaveChanges() == 1;
            }
        }
    }
}
