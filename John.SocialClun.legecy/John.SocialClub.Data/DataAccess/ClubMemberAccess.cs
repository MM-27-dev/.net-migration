// -----------------------------------------------------------------------
// <copyright file="ClubMemberAccess.cs" company="John">
// Socia Member club Demo ©2013
// </copyright>
// -----------------------------------------------------------------------

namespace John.SocialClub.Data.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using John.SocialClub.Data.DataModel;
    using John.SocialClub.Data.Enum;

    /// <summary>
    /// Data access class for ClubMember table (using static data)
    /// </summary>
    public class ClubMemberAccess : IClubMemberAccess
    {
        /// <summary>
        /// Static data storage for club members
        /// </summary>
        private static List<ClubMemberModel> staticMembers;

        /// <summary>
        /// Static counter for generating unique IDs
        /// </summary>
        private static int nextId = 1;

        /// <summary>
        /// Static constructor to initialize data
        /// </summary>
        static ClubMemberAccess()
        {
            InitializeStaticData();
        }

        /// <summary>
        /// Initialize static data with sample members
        /// </summary>
        private static void InitializeStaticData()
        {
            staticMembers = new List<ClubMemberModel>();

            // Random seed
            var rnd = new Random(12345); // Fixed seed for consistent data

            // Sample name parts
            string[] firstNames = { "Alex", "Jamie", "Taylor", "Morgan", "Jordan", "Casey", "Riley", "Avery", "Dakota", "Reese" };
            string[] lastNames = { "Smith", "Johnson", "Brown", "Taylor", "Anderson", "Thomas", "Jackson", "White", "Harris", "Martin" };

            int rows = 25; // number of initial members
            for (int i = 1; i <= rows; i++)
            {
                // Name
                string name = string.Format("{0} {1}", firstNames[rnd.Next(firstNames.Length)], lastNames[rnd.Next(lastNames.Length)]);

                // DateOfBirth: age between 18 and 70
                int age = rnd.Next(18, 71);
                var dob = DateTime.Today.AddYears(-age);
                // Add random days within the year to vary birthdays
                dob = dob.AddDays(rnd.Next(0, 365));

                // Random enum values
                var occupations = System.Enum.GetValues(typeof(Occupation)).Cast<Occupation>().ToArray();
                var maritalStatuses = System.Enum.GetValues(typeof(MaritalStatus)).Cast<MaritalStatus>().ToArray();
                var healthStatuses = System.Enum.GetValues(typeof(HealthStatus)).Cast<HealthStatus>().ToArray();

                var occupation = occupations[rnd.Next(occupations.Length)];
                var marital = maritalStatuses[rnd.Next(maritalStatuses.Length)];
                var health = healthStatuses[rnd.Next(healthStatuses.Length)];

                // Salary between 30k and 150k
                decimal salary = Math.Round((decimal)(rnd.NextDouble() * (150000 - 30000) + 30000), 2);

                // Number of children 0..6
                int children = rnd.Next(0, 7);

                staticMembers.Add(new ClubMemberModel
                {
                    Id = i,
                    Name = name,
                    DateOfBirth = dob,
                    Occupation = occupation,
                    MaritalStatus = marital,
                    HealthStatus = health,
                    Salary = salary,
                    NumberOfChildren = children
                });
            }

            nextId = rows + 1;
        }

        /// <summary>
        /// Method to get all club members (returns static data)
        /// </summary>
        /// <returns>Data table</returns>
        public static DataTable GetAllClubMembers()
        {
            var dataTable = new DataTable();

            // Define columns matching ClubMemberModel
            dataTable.Columns.Add("Id", typeof(int));
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("DateOfBirth", typeof(DateTime));
            dataTable.Columns.Add("Occupation", typeof(int));
            dataTable.Columns.Add("MaritalStatus", typeof(int));
            dataTable.Columns.Add("HealthStatus", typeof(int));
            dataTable.Columns.Add("Salary", typeof(decimal));
            dataTable.Columns.Add("NumberOfChildren", typeof(int));

            // Convert static members to DataTable
            foreach (var member in staticMembers)
            {
                var row = dataTable.NewRow();
                row["Id"] = member.Id;
                row["Name"] = member.Name;
                row["DateOfBirth"] = member.DateOfBirth;
                row["Occupation"] = (int)member.Occupation;
                row["MaritalStatus"] = (int)member.MaritalStatus;
                row["HealthStatus"] = (int)member.HealthStatus;
                row["Salary"] = member.Salary;
                row["NumberOfChildren"] = member.NumberOfChildren;
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }

        /// <summary>
        /// Instance method wrapper for static GetAllClubMembers
        /// </summary>
        /// <returns>Data table</returns>
        DataTable IClubMemberAccess.GetAllClubMembers()
        {
            return GetAllClubMembers();
        }

        /// <summary>
        /// Method to get club member by Id
        /// </summary>
        /// <param name="id">member id</param>
        /// <returns>Data row</returns>
        public DataRow GetClubMemberById(int id)
        {
            var member = staticMembers.FirstOrDefault(m => m.Id == id);
            if (member == null)
                return null;

            var dataTable = new DataTable();
            dataTable.Columns.Add("Id", typeof(int));
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("DateOfBirth", typeof(DateTime));
            dataTable.Columns.Add("Occupation", typeof(int));
            dataTable.Columns.Add("MaritalStatus", typeof(int));
            dataTable.Columns.Add("HealthStatus", typeof(int));
            dataTable.Columns.Add("Salary", typeof(decimal));
            dataTable.Columns.Add("NumberOfChildren", typeof(int));

            var row = dataTable.NewRow();
            row["Id"] = member.Id;
            row["Name"] = member.Name;
            row["DateOfBirth"] = member.DateOfBirth;
            row["Occupation"] = (int)member.Occupation;
            row["MaritalStatus"] = (int)member.MaritalStatus;
            row["HealthStatus"] = (int)member.HealthStatus;
            row["Salary"] = member.Salary;
            row["NumberOfChildren"] = member.NumberOfChildren;
            dataTable.Rows.Add(row);

            return row;
        }

        /// <summary>
        /// Method to search club members by multiple parameters
        /// </summary>
        /// <param name="occupation">occupation value</param>
        /// <param name="maritalStatus">marital status</param>
        /// <param name="operand">AND OR operand</param>
        /// <returns>Data table</returns>
        public DataTable SearchClubMembers(object occupation, object maritalStatus, string operand)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("Id", typeof(int));
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("DateOfBirth", typeof(DateTime));
            dataTable.Columns.Add("Occupation", typeof(int));
            dataTable.Columns.Add("MaritalStatus", typeof(int));
            dataTable.Columns.Add("HealthStatus", typeof(int));
            dataTable.Columns.Add("Salary", typeof(decimal));
            dataTable.Columns.Add("NumberOfChildren", typeof(int));

            var filteredMembers = staticMembers.AsQueryable();

            // Apply filters based on operand
            if (operand?.ToUpper() == "AND")
            {
                // Both conditions must be true
                if (occupation != null)
                {
                    int occupationValue = Convert.ToInt32(occupation);
                    filteredMembers = filteredMembers.Where(m => (int)m.Occupation == occupationValue);
                }
                if (maritalStatus != null)
                {
                    int maritalValue = Convert.ToInt32(maritalStatus);
                    filteredMembers = filteredMembers.Where(m => (int)m.MaritalStatus == maritalValue);
                }
            }
            else // OR operation or single condition
            {
                if (occupation != null && maritalStatus != null)
                {
                    int occupationValue = Convert.ToInt32(occupation);
                    int maritalValue = Convert.ToInt32(maritalStatus);
                    filteredMembers = filteredMembers.Where(m => 
                        (int)m.Occupation == occupationValue || 
                        (int)m.MaritalStatus == maritalValue);
                }
                else if (occupation != null)
                {
                    int occupationValue = Convert.ToInt32(occupation);
                    filteredMembers = filteredMembers.Where(m => (int)m.Occupation == occupationValue);
                }
                else if (maritalStatus != null)
                {
                    int maritalValue = Convert.ToInt32(maritalStatus);
                    filteredMembers = filteredMembers.Where(m => (int)m.MaritalStatus == maritalValue);
                }
            }

            // Convert filtered results to DataTable
            foreach (var member in filteredMembers)
            {
                var row = dataTable.NewRow();
                row["Id"] = member.Id;
                row["Name"] = member.Name;
                row["DateOfBirth"] = member.DateOfBirth;
                row["Occupation"] = (int)member.Occupation;
                row["MaritalStatus"] = (int)member.MaritalStatus;
                row["HealthStatus"] = (int)member.HealthStatus;
                row["Salary"] = member.Salary;
                row["NumberOfChildren"] = member.NumberOfChildren;
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }        

        /// <summary>
        /// Method to add new member
        /// </summary>
        /// <param name="clubMember">club member model</param>
        /// <returns>true or false</returns>
        public bool AddClubMember(ClubMemberModel clubMember)
        {
            try
            {
                // Assign new ID
                clubMember.Id = nextId++;
                
                // Add to static collection
                staticMembers.Add(new ClubMemberModel
                {
                    Id = clubMember.Id,
                    Name = clubMember.Name,
                    DateOfBirth = clubMember.DateOfBirth,
                    Occupation = clubMember.Occupation,
                    MaritalStatus = clubMember.MaritalStatus,
                    HealthStatus = clubMember.HealthStatus,
                    Salary = clubMember.Salary,
                    NumberOfChildren = clubMember.NumberOfChildren
                });

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Method to update club member
        /// </summary>
        /// <param name="clubMember">club member</param>
        /// <returns>true / false</returns>
        public bool UpdateClubMember(ClubMemberModel clubMember)
        {
            try
            {
                var existingMember = staticMembers.FirstOrDefault(m => m.Id == clubMember.Id);
                if (existingMember == null)
                    return false;

                // Update the existing member
                existingMember.Name = clubMember.Name;
                existingMember.DateOfBirth = clubMember.DateOfBirth;
                existingMember.Occupation = clubMember.Occupation;
                existingMember.MaritalStatus = clubMember.MaritalStatus;
                existingMember.HealthStatus = clubMember.HealthStatus;
                existingMember.Salary = clubMember.Salary;
                existingMember.NumberOfChildren = clubMember.NumberOfChildren;

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Method to delete a club member
        /// </summary>
        /// <param name="id">member id</param>
        /// <returns>true / false</returns>
        public bool DeleteClubMember(int id)
        {
            try
            {
                var memberToRemove = staticMembers.FirstOrDefault(m => m.Id == id);
                if (memberToRemove == null)
                    return false;

                staticMembers.Remove(memberToRemove);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
