using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traveler.DALayer.Models;
namespace Traveler.DALayer
{
    public class TravelerRepo
    {
            public TravelerDbContext Context { get; set; }


            public TravelerRepo()
            {
                Context = new TravelerDbContext();
            }


            // Method to register new Customer of type DAL.Models.Customer

            // public int RegisterNewCustomer(string emailId, string firstName,string lastName,string userPassword,string gender
            //   ,int contactNumber ,DateTime dateOfBirth,string address)
            public int RegisterNewCustomer(Customer newCust)
            {
                int result;
                try
                {
                    SqlParameter prmFirstName = new SqlParameter("@firstName", newCust.FirstName);
                    SqlParameter prmLastName = new SqlParameter("@lastName", newCust.LastName);
                    SqlParameter prmPassword = new SqlParameter("@userPassword", newCust.UserPassword);
                    SqlParameter prmGender = new SqlParameter("@gender", newCust.Gender);
                    SqlParameter prmEmailId = new SqlParameter("@emailId", newCust.EmailId);
                    SqlParameter prmDob = new SqlParameter("@dateOfBirth", newCust.DateOfBirth);
                    SqlParameter prmNumber = new SqlParameter("@contactNumber", newCust.ContactNumber);
                    SqlParameter prmAddress = new SqlParameter("@address", newCust.Address);

                    SqlParameter prmReturnResult = new SqlParameter("@ReturnResult", System.Data.SqlDbType.Int);
                    prmReturnResult.Direction = System.Data.ParameterDirection.Output;

                    result = Context.Database.ExecuteSqlRaw("EXEC @ReturnResult= usp_RegisterCustomer @emailId,@firstName,@lastName,@userPassword, " +
                        "@gender,@contactNumber,@dateOfBirth,@address",
                        prmReturnResult, prmEmailId, prmFirstName, prmLastName, prmPassword, prmGender, prmNumber, prmDob, prmAddress);
                    if (result > 0)
                    {
                        return result;
                    }
                    else
                    {
                        result = -98;
                        return result;
                    }
                }
                catch (Exception e)
                {
                    result = -99;
                    return result;
                }

            }

            // Method to validate a User
            public int ValidateLoginCustomer(string emailId, string password)
            {
                int roleId = 0;
                try
                {
                    var objUser = (from usr in Context.Customers
                                   where usr.EmailId == emailId && usr.UserPassword == password
                                   select usr.Role).FirstOrDefault<Role>();

                    if (objUser != null)
                    {
                        roleId = objUser.RoleId;

                    }
                    else
                    {
                        roleId = 0;
                    }
                }
                catch (Exception)
                {
                    roleId = -99;
                }
                return roleId;
            }


            public bool EditProfile(Customer cust)
            {
                bool status = false;
                Customer cust1 = Context.Customers.Find(cust.EmailId);
                try
                {
                    if (cust1 != null)
                    {
                        cust1.FirstName = cust.FirstName;
                        cust1.LastName = cust.LastName;
                        cust1.ContactNumber = cust.ContactNumber;
                        cust1.Address = cust.Address;
                        cust1.Gender = cust.Gender;
                        cust1.DateOfBirth = cust.DateOfBirth;
                        Context.SaveChanges();
                        status = true;
                    }
                    else
                    {
                        status = false;
                    }
                }
                catch (Exception e)
                {
                    status = false;
                    Console.WriteLine(e.Message);
                }
                return status;
            }

            public List<Package> GetPackages()
            {
                List<Package> package;
                try
                {
                    package = Context.Packages.FromSqlRaw("SELECT * FROM dbo.ufn_ViewAllPackages()").ToList();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    package = null;
                }
                return package;
            }

            public Customer GetCustomerById(string email)
            {
                Customer cust;
                try
                {

                    cust = (from usr in Context.Customers
                            where usr.EmailId == email
                            select usr).FirstOrDefault();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    cust = null;
                }
                return cust;
            }

            public List<PackageCategory> GetPackageCategories()
            {
                {
                    List<PackageCategory> obj = null;
                    try
                    {
                        obj = (from a in Context.PackageCategories select a).ToList();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        obj = null;
                    }
                    return obj;
                }

            }

            public List<Vehicle> GetVehicles()
            {
                {
                    List<Vehicle> obj = null;
                    try
                    {
                        obj = (from a in Context.Vehicles select a).ToList();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        obj = null;
                    }
                    return obj;
                }

            }

            public List<Hotel> GetHotels()
            {
                {
                    List<Hotel> obj = null;
                    try
                    {
                        obj = (from a in Context.Hotels select a).ToList();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        obj = null;
                    }
                    return obj;
                }

            }

            public List<Package> GetPackageByCategoryId(int categoryId)
            {
                List<Package> obj = null;
                try
                {
                    obj = (from a in Context.Packages where a.PackageCategoryId == categoryId select a).ToList();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    obj = null;
                }
                return obj;
            }

            public List<PackageDetail> GetPackageDetailsByPackageId(int packageId)
            {
                List<PackageDetail> obj = null;
                try
                {
                    obj = (from a in Context.PackageDetails where a.PackageId == packageId select a).ToList();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    obj = null;
                }
                return obj;
            }

            public int EmployeeLogin(string emailId, string password)
            {
                int roleId = 0;
                try
                {
                    var objUser = (from usr in Context.Employees
                                   where usr.EmailId == emailId && usr.Password == password
                                   select usr.Role).FirstOrDefault<Role>();

                    if (objUser != null)
                    {
                        roleId = objUser.RoleId;

                    }
                    else
                    {
                        roleId = 0;
                    }
                }
                catch (Exception)
                {
                    roleId = -99;
                }
                return roleId;
            }

            public Accomodation GetAccomodationByBookingId(int bookingId)
            {
                Accomodation obj = null;
                try
                {
                    obj = (from a in Context.Accomodations
                           where a.BookingId == bookingId
                           select a
                           ).FirstOrDefault();
                }
                catch (Exception)
                {
                    obj = null;
                }
                return obj;
            }

            // Insert bookPackage object using linq query
            public bool AddBookPackage(BookPackage obj)
            {
                bool result = false;
                try
                {
                    Context.BookPackages.Add(obj);
                    Context.SaveChanges();
                    result = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    result = false;
                }
                return result;
            }

            // Insert bookPackage object using stored procedure
            public int BookPackageBySP(BookPackage bookpkg)
            {

                int bookingId = 0;
                int result = -1;
                try
                {
                    SqlParameter prmEmailId = new SqlParameter("@EmailId", bookpkg.EmailId);
                    SqlParameter prmContactNumber = new SqlParameter("@ContactNumber", bookpkg.ContactNumber);
                    SqlParameter prmAddress = new SqlParameter("@Address", bookpkg.Address);
                    SqlParameter prmDateOfTravel = new SqlParameter("@DateOfTravel", bookpkg.DateOfTravel);
                    SqlParameter prmNumberOfAdults = new SqlParameter("@NumberOfAdults", bookpkg.NumberOfAdults);
                    SqlParameter prmNumberOfChildren = new SqlParameter("@NumberOfChildren", bookpkg.NumberOfChildren);
                    SqlParameter prmStatus = new SqlParameter("@Status", bookpkg.Status);
                    SqlParameter prmPackageId = new SqlParameter("@PackageId", bookpkg.PackageId);

                    SqlParameter prmBookingId = new SqlParameter("@BookingId", System.Data.SqlDbType.BigInt);
                    prmBookingId.Direction = System.Data.ParameterDirection.Output;

                    SqlParameter prmReturnResult = new SqlParameter("@ReturnResult", System.Data.SqlDbType.Int);
                    prmReturnResult.Direction = System.Data.ParameterDirection.Output;

                    result = Context.Database.ExecuteSqlRaw("EXEC @ReturnResult= usp_BookPackage @EmailId,@ContactNumber,@Address,@DateOfTravel," +
                        "@NumberOfAdults,@NumberOfChildren,@Status,@PackageId,@BookingId OUT", prmReturnResult, prmEmailId, prmContactNumber,
                      prmAddress, prmDateOfTravel, prmNumberOfAdults, prmNumberOfChildren, prmStatus, prmPackageId, prmBookingId);

                    if (result > 0)
                    {
                        bookingId = Convert.ToInt32(prmBookingId.Value);

                    }
                    else
                    {
                        bookingId = 0;

                    }
                }
                catch (Exception ex)
                {
                    bookingId = 0;
                    Console.WriteLine(ex.Message);

                }
                return bookingId;

            }

            public bool AddAccomodation(Accomodation obj)
            {
                bool result;
                try
                {
                    Context.Accomodations.Add(obj);
                    Context.SaveChanges();
                    result = true;

                }
                catch (Exception)
                {
                    result = false;
                }
                return result;
            }


            //Accommodation selection
            public string GetCitiesByPackageDetailsId(int bookId)
            {
                string city = null;
                int pkgid = 0;
                try
                {
                    pkgid = (int)(from bookpackage in Context.BookPackages where bookpackage.BookingId == bookId select bookpackage.PackageId).FirstOrDefault();
                    city = (from package in Context.PackageDetails
                            where package.PackageDetailsId == pkgid
                            select package.PlacesToVisit).FirstOrDefault();
                }
                catch (Exception)
                {
                    city = null;
                }
                return city;
            }

            public List<int> GetHotelRatingByCity(string city)
            {
                List<int> rating = null;
                try
                {
                    rating = (from hotel in Context.Hotels
                              where hotel.City == city
                              group hotel by hotel.HotelRating into g
                              select g.Key).ToList();
                }
                catch (Exception)
                {
                    rating = null;
                }
                return rating;
            }

            public List<string> GetHotelsByCityAndRating(string city, int rating)
            {
                List<string> hotels = null;
                try
                {
                    hotels = (from hotel in Context.Hotels
                              where hotel.City == city && hotel.HotelRating == rating
                              select hotel.HotelName).ToList();
                }
                catch (Exception)
                {
                    hotels = null;
                }
                return hotels;
            }

            //----------------------payment baad k liye----------------------------------------------
            public bool PaymentMethod(Payment payment)
            {
                bool result;
                try
                {
                    Context.Payments.Add(payment);
                    Context.SaveChanges();
                    result = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    result = false;
                }
                return result;
            }
            //int result;
            //try
            //{
            //    SqlParameter prmPaymentId = new SqlParameter("@PaymentId", payment.PaymentId);
            //    SqlParameter prmBookingId = new SqlParameter("@BookingId", payment.BookingId);
            //    SqlParameter prmTotalCost = new SqlParameter("@TotalCost", payment.TotalAmount);

            //    SqlParameter prmReturnResult = new SqlParameter("@ReturnResult", System.Data.SqlDbType.Int);
            //    prmReturnResult.Direction = System.Data.ParameterDirection.Output;

            //    result = Context.Database.ExecuteSqlRaw("EXEC @ReturnResult= usp_bookingCost @BookingId",
            //        prmReturnResult, prmBookingId);
            //    if (result > 0)
            //    {
            //        return result;
            //    }
            //    else
            //    {
            //        result = -98;
            //        return result;
            //    }
            //}
            //catch (Exception e)
            //{
            //    result = -99;
            //    return result;

            //}
            //Payment obj=new Payemnt;


            public bool AddCustomerCare(CustomerCare c)
            {
                bool result = false;
                try
                {
                    Context.CustomerCares.Add(c);
                    Context.SaveChanges();
                    result = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    result = false;
                }
                return result;
            }

            public bool AddNewHotel(Hotel obj)
            {
                bool result;
                try
                {
                    Context.Hotels.Add(obj);
                    Context.SaveChanges();
                    result = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    result = false;
                }
                return result;
            }

            public bool AddNewVehicle(Vehicle obj)
            {
                bool result;
                try
                {
                    Context.Vehicles.Add(obj);
                    Context.SaveChanges();
                    result = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    result = false;
                }
                return result;
            }

            public int GetAssigne()
            {
                Random random = new Random();
                var employee = (from ab in Context.Employees select ab.EmpId).ToList();
                int[] empId = employee.ToArray();
                int obj = random.Next(0, (empId.Length));
                return empId[obj];

            }

            public int TotalAmount(int bid)
            {
                int total = 0; int p = 0;
                try
                {
                    var details1 = (from pkgDetail in Context.PackageDetails
                                    join bookpackage in Context.BookPackages
                                    on pkgDetail.PackageDetailsId equals bookpackage.PackageId
                                    where bookpackage.BookingId == bid
                                    select new { pkgDetail.PricePerAdult, bookpackage.NumberOfAdults, bookpackage.NumberOfChildren }).FirstOrDefault();

                    var details2 = (from accommodation in Context.Accomodations
                                    join bookpackage in Context.BookPackages
                                    on accommodation.BookingId equals bookpackage.BookingId
                                    where bookpackage.BookingId == bid
                                    select new { accommodation.Price }).FirstOrDefault();

                    total = (int)(details1.PricePerAdult * details1.NumberOfAdults + (details1.PricePerAdult * details1.NumberOfChildren) / 2);
                    total += (int)details2.Price;


                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                return total;
            }

            //public List<ViewBookings> ViewBookedPackages(string emailId)
            //{
            //    List<ViewBookings> result = null;

            //    try
            //    {
            //        SqlParameter email = new SqlParameter("@EmailId", emailId);
            //        result = Context.ViewBookings.FromSqlRaw("Select * From dbo.ufn_ViewBookedPackages(@EmailId)", email).ToList();

            //    }
            //    catch (Exception ex)
            //    {

            //        Console.WriteLine(ex.Message);
            //        result= null;
            //    }
            //    return result;

            //}

            public List<BookPackage> ViewBookedPackages(string emailId)
            {
                List<BookPackage> result = null;

                try
                {
                    SqlParameter email = new SqlParameter("@EmailId", emailId);
                    result = Context.BookPackages.FromSqlRaw("Select * From dbo.ufn_ViewBookedPackages(@EmailId)", email).ToList();

                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                    result = null;
                }
                return result;

            }

            public int GetHotelCost(string hotelName, string roomtype)
            {
                int cost = 0;
                try
                {
                    if (roomtype == "Single")
                    {
                        cost = (int)(from hotel in Context.Hotels where hotel.HotelName == hotelName select hotel.SingleRoomPrice).FirstOrDefault();
                    }
                    else if (roomtype == "Double")
                    {
                        cost = (int)(from hotel in Context.Hotels where hotel.HotelName == hotelName select hotel.DoubleRoomPrice).FirstOrDefault();
                    }
                    else if (roomtype == "Deluxe")
                    {
                        cost = (int)(from hotel in Context.Hotels where hotel.HotelName == hotelName select hotel.DeluxeeRoomPrice).FirstOrDefault();
                    }
                    else if (roomtype == "Suite")
                    {
                        cost = (int)(from hotel in Context.Hotels where hotel.HotelName == hotelName select hotel.SuiteRoomPrice).FirstOrDefault();
                    }

                }
                catch (Exception e)
                {

                    cost = 0;
                }
                return cost;
            }
        }
}
