using RampUpProjectBE.DAL;
using RampUpProjectBE.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace RampUpProjectBE.Services {
    public class ReservationService {
        private string rampConnectionString;

        public ReservationService(string rampConnectionString) {
            this.rampConnectionString = rampConnectionString;
        }

        public ReservationDTO GetReservation(int reservationID) {
            bool lockWasTaken = false;
            ReservationDTO reservationDTO = null;

            using (RampUpProjectEntities dbContext = new RampUpProjectEntities(rampConnectionString)) {
                try {
                    Monitor.Enter(dbContext, ref lockWasTaken);

                    Reservation reservation = dbContext.Reservations.Where(x => x.Reservation_Id == reservationID).FirstOrDefault();

                    if (reservation != null) {
                        reservationDTO = ConvertReservationToReservationDTO(reservation);
                    }

                    if (lockWasTaken) {
                        Monitor.Exit(dbContext);
                        lockWasTaken = false;
                    }

                } catch (Exception) {
                    if (lockWasTaken) {
                        Monitor.Exit(dbContext);
                        lockWasTaken = false;
                    }

                    throw;
                }

                return reservationDTO;
            }
        }

        public List<ReservationDTO> GetMyReservations(int userID) {
            bool lockWasTaken = false;
            List<ReservationDTO> reservations = new List<ReservationDTO>();

            using (RampUpProjectEntities dbContext = new RampUpProjectEntities(rampConnectionString)) {
                try {
                    Monitor.Enter(dbContext, ref lockWasTaken);

                    reservations = dbContext.Reservations.Where(x => x.User_Id == userID).Select(x => new ReservationDTO() { Reservation_Id = x.Reservation_Id, Creation_Date = x.Creation_Date, Start_Time = x.Start_Time, Date = x.Date, End_Time = x.End_Time,  User_Id = x.User_Id, Branch_Name = x.Field.Branch.Name, Field_Number = x.Field.Number }).ToList();

                    if (lockWasTaken) {
                        Monitor.Exit(dbContext);
                        lockWasTaken = false;
                    }

                } catch (Exception) {
                    if (lockWasTaken) {
                        Monitor.Exit(dbContext);
                        lockWasTaken = false;
                    }

                    throw;
                }

                return reservations;
            }
        }

        public List<ReservationDTO> GetReservationsPerDay(string date, int fieldID) {
            bool lockWasTaken = false;
            List<ReservationDTO> reservations = new List<ReservationDTO>();
            DateTime datetime = Convert.ToDateTime(date);

            using (RampUpProjectEntities dbContext = new RampUpProjectEntities(rampConnectionString)) {
                try {
                    Monitor.Enter(dbContext, ref lockWasTaken);
                    reservations = dbContext.Reservations.Where(x => x.Date == datetime && x.Field_Id == fieldID).Select(x => new ReservationDTO() { Reservation_Id = x.Reservation_Id, Creation_Date = x.Creation_Date, Start_Time = x.Start_Time, Date = x.Date, End_Time = x.End_Time, User_Id = x.User_Id }).ToList();

                    if (lockWasTaken) {
                        Monitor.Exit(dbContext);
                        lockWasTaken = false;
                    }

                } catch (Exception) {
                    if (lockWasTaken) {
                        Monitor.Exit(dbContext);
                        lockWasTaken = false;
                    }

                    throw;
                }

                return reservations;
            }
        }

        public int AddReservation(ReservationDTO reservationDTO) {
            try {
                Reservation reservation = ConvertReservationDTOToReservation(reservationDTO);

                using (RampUpProjectEntities dbContext = new RampUpProjectEntities(rampConnectionString)) {
                    dbContext.Reservations.Add(reservation);
                    dbContext.SaveChanges();
                }

                return reservation.Reservation_Id;
            } catch (Exception) {
                throw;
            }
        }

        public void RemoveReservation(int reservationID) {
            try {
                Reservation reservation = new Reservation() { Reservation_Id = reservationID };

                using (RampUpProjectEntities dbContext = new RampUpProjectEntities(rampConnectionString)) {
                    dbContext.Reservations.Attach(reservation);
                    dbContext.Reservations.Remove(reservation);
                    dbContext.SaveChanges();
                }
            } catch (Exception) {
                throw;
            }
        }

        private static Reservation ConvertReservationDTOToReservation(ReservationDTO reservationDTO) {
            return new Reservation {
                Reservation_Id = reservationDTO.Reservation_Id,
                Date = reservationDTO.Date,
                Creation_Date = reservationDTO.Creation_Date,
                Start_Time = reservationDTO.Start_Time,
                End_Time = reservationDTO.End_Time,
                User_Id = reservationDTO.User_Id,
                Field_Id = reservationDTO.Field_Id
            };
        }

        private static ReservationDTO ConvertReservationToReservationDTO(Reservation reservation) {
            return new ReservationDTO {
                Reservation_Id = reservation.Reservation_Id,
                Date = reservation.Date,
                Creation_Date = reservation.Creation_Date,
                Start_Time = reservation.Start_Time,
                End_Time = reservation.End_Time,
                User_Id = reservation.User_Id,
                Field_Id = reservation.Field_Id
            };
        }
    }
}