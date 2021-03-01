//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Web;

//namespace Wildlife.Models
//{
//    //
//    // Summary:
//    //     Represents a civic address. A civic address can include fields such as street
//    //     address, postal code, state/province, and country or region.
//    public class CivicAddress123
//    {
//        //
//        // Summary:
//        //     Represents a System.Device.Location.CivicAddress that contains no data.
//        public static readonly CivicAddress123 Unknown;

//        //
//        // Summary:
//        //     Initializes a new instance of the System.Device.Location.CivicAddress class.
//        public CivicAddress123() { }
//        //
//        // Summary:
//        //     Initializes a new instance of the System.Device.Location.CivicAddress class using
//        //     address information.
//        //
//        // Parameters:
//        //   addressLine1:
//        //     A string containing the first line of the street address.
//        //
//        //   addressLine2:
//        //     A string containing the second line of the street address.
//        //
//        //   building:
//        //     A string containing the building name or number.
//        //
//        //   city:
//        //     A string containing the city.
//        //
//        //   countryRegion:
//        //     A string containing the country or region.
//        //
//        //   floorLevel:
//        //     A string containing the floor number.
//        //
//        //   postalCode:
//        //     A string containing the postal code.
//        //
//        //   stateProvince:
//        //     A string containing the state or province.
//        //
//        // Exceptions:
//        //   T:System.ArgumentException:
//        //     At least one parameter must be a non-empty string.
//        public CivicAddress123(string addressLine1, string addressLine2, string building, string city, string countryRegion, string floorLevel, string postalCode, string stateProvince)
//        {
//            AddressLine1 = addressLine1;
//            AddressLine2 = addressLine2;
//            Building = building;
//            City = city;
//            CountryRegion = countryRegion;
//            FloorLevel = floorLevel;
//            PostalCode = postalCode;
//            StateProvince = stateProvince;
//        }
//        //
//        // Summary:
//        //     Gets or sets the first line of the address.
//        //
//        // Returns:
//        //     Returns the first line of the address. If the data is not provided, returns System.String.Empty.
//        public string AddressLine1 { get; set; }
//        //
//        // Summary:
//        //     Gets or sets the second line of the address.
//        //
//        // Returns:
//        //     Returns the second line of the address. If the data is not provided, returns
//        //     System.String.Empty.
//        public string AddressLine2 { get; set; }
//        //
//        // Summary:
//        //     Gets or sets the building name or number.
//        //
//        // Returns:
//        //     Returns the building name or number. If the data is not provided, returns System.String.Empty.
//        public string Building { get; set; }
//        //
//        // Summary:
//        //     Gets or sets the name of the city.
//        //
//        // Returns:
//        //     Returns the name of the city. If the data is not provided, returns System.String.Empty.
//        public string City { get; set; }
//        //
//        // Summary:
//        //     Gets or sets the country or region of the location.
//        //
//        // Returns:
//        //     Returns the country or region code. If the data is not provided, returns System.String.Empty.
//        public string CountryRegion { get; set; }
//        //
//        // Summary:
//        //     Gets or sets the floor level of the location.
//        //
//        // Returns:
//        //     Returns a string that contains the floor level. If the data is not provided,
//        //     returns System.String.Empty.
//        public string FloorLevel { get; set; }
//        //
//        // Summary:
//        //     Gets or sets the postal code of the location.
//        //
//        // Returns:
//        //     Returns the postal code of the location. If the data is not provided, returns
//        //     System.String.Empty.
//        public string PostalCode { get; set; }
//        //
//        // Summary:
//        //     Gets or sets the state or province of the location.
//        //
//        // Returns:
//        //     Returns the state or province of the location. If the data is not provided, returns
//        //     System.String.Empty.
//        public string StateProvince { get; set; }
//        //
//        // Summary:
//        //     Gets a value that indicates whether the System.Device.Location.CivicAddress contains
//        //     data.
//        //
//        // Returns:
//        //     true if the System.Device.Location.CivicAddress is equal to System.Device.Location.CivicAddress.Unknown;
//        //     otherwise, false.
//        public bool IsUnknown { get; }
//    }
//}