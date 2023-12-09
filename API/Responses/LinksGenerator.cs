//George Michael 991652543
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Responses
{
    public class LinksGenerator
    {
        public static Dictionary<string , string>  GenrateLinks (string baseURL, int currentPage, int totalRecords , int pageSize)
        {

            var result = new Dictionary<string, string >();

            int totalPages =(int)Math.Ceiling((decimal)totalRecords/(decimal)pageSize) ;

            result.Add("First" , $"{baseURL}?pageNumber=1&pagesize={pageSize}"); 

            if(currentPage >1)
            {
                result.Add("Prev" , $"{baseURL}?pageNumber={currentPage -1}&pageSize={pageSize}");
            }

            if(currentPage <totalPages)
            {
                result.Add("Next", $"{baseURL}?pageNumber={currentPage +1}&pageSize={pageSize}");
            }


            result.Add("Last", $"{baseURL}?pageNumber={totalPages}&pageSize={pageSize}");
            return result;

        }
    }
}