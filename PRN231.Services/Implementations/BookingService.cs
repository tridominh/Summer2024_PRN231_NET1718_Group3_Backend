﻿using PRN231.Constant;
using PRN231.Models;
using PRN231.Models.DTOs.Request;
using PRN231.Models.DTOs.Response;
using PRN231.Repositories.Interfaces;

namespace PRN231.Services.Implementations
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IBookingUserRepository _bookingUserRepository;
        private readonly ISubjectLevelRepository _subjectLevelRepository;

        public BookingService(IBookingRepository bookingRepository,
            IBookingUserRepository bookingUserRepository,
            ISubjectLevelRepository subjectLevelRepository)
        {
            _bookingRepository = bookingRepository;
            _bookingUserRepository = bookingUserRepository;
            _subjectLevelRepository = subjectLevelRepository;
        }

        public async Task<List<Booking>> GetAllBookings()
        {
            return (List<Booking>)await _bookingRepository.GetAll();
        }

        public async Task<CreateBookingResponse> CreateBooking(CreateBookingRequest createBookingRequest)
        {
            SubjectLevel subjectLevel = _subjectLevelRepository
                .FindSubjectLevelBySubjectIdAndLevelId(createBookingRequest.SubjectId, createBookingRequest.LevelId);

            if(subjectLevel == null)
            {
                throw new Exception("Subject Level not found");
            }

            Booking booking = new Booking
            {
                SubjectLevelId = subjectLevel.Id,
                Price = 0,
                PaymentMethod = PaymentMethodConstant.UNDEFINED,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                Status = BookingStatusConstant.PENDING
            };

            try
            {
                Booking addedBooking = await _bookingRepository.Add(booking);

                BookingUser bookingUser = new BookingUser
                {
                    UserId = createBookingRequest.UserId,
                    BookingId = addedBooking.Id,
                    Description = createBookingRequest.Description,
                    Role = RoleEnum.STUDENT,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    Status = StatusConstant.ACTIVE
                };

                BookingUser savedBookingUser = await _bookingUserRepository.Add(bookingUser);

                CreateBookingResponse bookingResponse = new CreateBookingResponse
                {
                    SubjectLevelId = addedBooking.SubjectLevelId,
                    UserId = savedBookingUser.UserId,
                    Role = savedBookingUser.Role,
                    Description = savedBookingUser.Description
                };

                return bookingResponse;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UpdateBookingResponse> UpdateBooking(UpdateBookingRequest updateBookingRequest)
        {
            try
            {
                Booking existingBooking = _bookingRepository.GetAll().Result
                    .FirstOrDefault(b => b.Id ==  updateBookingRequest.BookingId);
                existingBooking.SubjectLevelId = updateBookingRequest.SubjectLevelId;
                existingBooking.Price = updateBookingRequest.Price;
                existingBooking.PaymentMethod = updateBookingRequest.PaymentMethod;
                existingBooking.Status = updateBookingRequest.Status;

                Booking updatedBooking = await _bookingRepository.Update(existingBooking);

                UpdateBookingResponse bookingResponse = new UpdateBookingResponse
                {
                    SubjectLevelId = updatedBooking.SubjectLevelId,
                    Price = updatedBooking.Price,
                    PaymentMethod = updatedBooking.PaymentMethod,
                    Status = updatedBooking.Status
                };

                return bookingResponse;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
