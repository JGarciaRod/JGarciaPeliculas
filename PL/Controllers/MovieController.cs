using Microsoft.AspNetCore.Mvc;
using PL.Models;
using System.Text.Json;

namespace PL.Controllers
{
    public class MovieController : Controller
    {
        public IActionResult GetAllPopular(int? page)
        {
            Root root = new Root();
            root.results = new List<Movie>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.themoviedb.org/3/movie/popular");

                string url = "";
                string urlPoster = "https://image.tmdb.org/t/p/w300_and_h450_bestv2";

                if (page == null)
                {
                    url = "?api_key=a2ef5167fa8497bbcb856668486ae1b9";
                }
                else
                {
                    url = "?api_key=a2ef5167fa8497bbcb856668486ae1b9&page="+page;
                }

                var responseTask = client.GetAsync(url);
                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Root>();
                    readTask.Wait();

                    foreach ( var item in readTask.Result.results)
                    {
                        Movie movie = new Movie();

                        movie.backdrop_path = item.backdrop_path;
                        movie.adult = item.adult;
                        movie.genre_ids = item.genre_ids;
                        movie.id = item.id;
                        movie.original_language = item.original_language;
                        movie.original_title = item.original_title;
                        movie.overview = item.overview;
                        movie.popularity = item.popularity;
                        movie.poster_path = urlPoster + item.poster_path;
                        movie.release_date = item.release_date;
                        movie.title = item.title;
                        movie.video = item.video;
                        movie.vote_average = item.vote_average;
                        movie.vote_count = item.vote_count;

                        root.results.Add(movie);
                    }

                }

            }
            ViewBag.page = page;
            return View(root);
        }

        public IActionResult Favoritos(int IdMovie, bool favorite)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.themoviedb.org/3/");

                var url = "account/20522131/favorite?api_key=a2ef5167fa8497bbcb856668486ae1b9&session_id=b74af68dfa4ffbb975aca85a3d3f873f19a9ee3a";
                
               var json = new {
                    media_type = "movie",
                    media_id =  (int)IdMovie,
                    favorite = favorite
                };

                var responsTask = client.PostAsJsonAsync(url,json);
                responsTask.Wait();

                var result = responsTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    ViewBag.Mensaje = "Exito en la operacion";
                }
                else{
                    ViewBag.Error = result.ToString();
                }
            }

            return PartialView("Modal");
        }

        public IActionResult GetAllFavoritos()
        {
            Root root = new Root();
            root.results = new List<Movie>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.themoviedb.org/3/account/20522131/favorite/movies");

                string url = "?api_key=a2ef5167fa8497bbcb856668486ae1b9&session_id=b74af68dfa4ffbb975aca85a3d3f873f19a9ee3a";
                string urlPoster = "https://image.tmdb.org/t/p/w300_and_h450_bestv2";

                var responseTask = client.GetAsync(url);
                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Root>();
                    responseTask.Wait();
                    foreach (var item in readTask.Result.results)
                    {
                        Movie movie = new Movie();

                        movie.backdrop_path = item.backdrop_path;
                        movie.adult = item.adult;
                        movie.genre_ids = item.genre_ids;
                        movie.id = item.id;
                        movie.original_language = item.original_language;
                        movie.original_title = item.original_title;
                        movie.overview = item.overview;
                        movie.popularity = item.popularity;
                        movie.poster_path = urlPoster + item.poster_path;
                        movie.release_date = item.release_date;
                        movie.title = item.title;
                        movie.video = item.video;
                        movie.vote_average = item.vote_average;
                        movie.vote_count = item.vote_count;

                        root.results.Add(movie);
                    }
                }
            }
            return View(root);
        }
    }
}
