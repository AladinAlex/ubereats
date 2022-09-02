using Microsoft.AspNetCore.Mvc;

namespace ubereats.Interfaces
{
    public interface IRestaurant
    {
        ActionResult GetImage(int id);
    }
}
