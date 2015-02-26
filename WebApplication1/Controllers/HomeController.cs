using Agent2048;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public JsonResult GetBestMove(string[] arr)
        {
            State2048 state = new State2048(4, 4);
            foreach (var item in arr)
            {
                string[] data = item.Split('-');
                int x = int.Parse(data[0]) - 1;
                int y = int.Parse(data[1]) - 1;
                int tileVal =Convert.ToInt32(Math.Log(double.Parse(data[2]), 2)) ;
                if (tileVal > state.grid[y, x])
                {
                    state.grid[y, x] = tileVal;
                }
            }

            double bestScore = double.MaxValue;
			List<StateTrans> movesBest = new List<StateTrans>();

            List<StateTrans> moves = state.getAllMoveStates();
			foreach(StateTrans move in moves) 
			{
				double rating = State2048.alphabetarate(move.state, 13, double.MaxValue, double.MinValue, false);
				if( rating < bestScore )
				{
					bestScore = rating;
					movesBest.Clear();
				}
					
				if( rating == bestScore )
				{
					movesBest.Add(move);
				}
			}
				
			if( movesBest.Count == 0 )
				return Json(-1, JsonRequestBehavior.AllowGet);
				

            return Json(movesBest[0].dir, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}