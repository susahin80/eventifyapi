using Eventify.Core.Domain;
using Eventify.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventify.Persistence
{
    public class Seed
    {
        public static async Task SeedData(EventifyDbContext context)
        {

            var sportsGuid = Guid.NewGuid();
            var musicsGuid = Guid.NewGuid();
            var foodsGuid = Guid.NewGuid();
            var danceGuid = Guid.NewGuid();

            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {

                    new Category
                    {
                        Id = sportsGuid,
                        Name = "Sports"
                    },
                       new Category
                    {
                       Id = musicsGuid,
                        Name = "Music"
                    },
                          new Category
                    {
                        Id = foodsGuid,
                        Name = "Food"
                    },
                             new Category
                    {
                          Id = danceGuid,
                        Name = "Dance"
                    },
                };

                await context.Categories.AddRangeAsync(categories);
                await context.SaveChangesAsync();
            }

            if (!context.Users.Any())
            {
                var users = new List<User>
                {
                    new User
                    {
                        Username = "Tom",
                        Email = "tom@test.com",
                        Password = Hasher<User>.Hash(null, "123123"),
                        Gender = "M",
                        BirthDate = new DateTime(2002, 03, 18),
                        Events = new List<Event>
                        {
                            new Event
                            {
                                Title = "Tom's Chess Event",
                                Description = "Tom's Chess Event Description...",
                                PlaceName = "Club Gambit",
                                CategoryId = sportsGuid,
                                StartDate = new DateTime(2020, 06, 15, 16, 0, 0),
                                EndDate = new DateTime(2020, 06, 15, 20, 0, 0),
                                IsActive = true,
                                MaxNumberOfPeople = 4
                            },
                            new Event
                            {
                                Title = "Tom's Music Event",
                                Description = "Tom's Music Event Description...",
                                PlaceName = "Club Music",
                                CategoryId = musicsGuid,
                                StartDate = new DateTime(2020, 06, 19, 21, 0, 0),
                                EndDate = new DateTime(2020, 06, 19, 23, 0, 0),
                                IsActive = true,
                                MaxAgeLimit = 40
                            },
                            new Event
                            {
                                Title = "Tom's Food Event",
                                Description = "Tom's Food Event Description...",
                                PlaceName = "Cook House",
                                CategoryId = foodsGuid,
                                StartDate = new DateTime(2020, 06, 28, 16, 0, 0),
                                EndDate = new DateTime(2020, 06, 28, 18, 0, 0),
                                IsActive = true,
                                Price = new decimal(20),
                            },
                            new Event
                            {
                                Title = "Tom's Dance Event For 30s",
                                Description = "Tom's Dance Event Description...",
                                PlaceName = "Club Dance",
                                CategoryId = danceGuid,
                                StartDate = new DateTime(2020, 07, 11, 22, 0, 0),
                                EndDate = new DateTime(2020, 07, 12, 02, 0, 0),
                                IsActive = true,
                                MinAgeLimit = 30,
                                MaxAgeLimit = 39
                            }
                        }
                    },
                    new User
                    {
                        Username = "Bob",
                        Email = "bob@test.com",
                        Password = Hasher<User>.Hash(null, "123123"),
                        Gender = "M",
                        BirthDate = new DateTime(1980, 09, 12),
                        Events = new List<Event>
                        {
                            new Event
                            {
                                Title = "Bob's Football Event",
                                Description = "Bob's Football Event Description...",
                                PlaceName = "Football Field",
                                CategoryId = sportsGuid,
                                StartDate = new DateTime(2020, 06, 15, 16, 0, 0),
                                EndDate = new DateTime(2020, 06, 15, 18, 0, 0),
                                IsActive = true,
                                MaxNumberOfPeople = 8
                            },
                            new Event
                            {
                                Title = "Bob's Music Event",
                                Description = "Bob's Music Event Description...",
                                PlaceName = "Club Music",
                                CategoryId = musicsGuid,
                                StartDate = new DateTime(2020, 06, 20, 19, 0, 0),
                                EndDate = new DateTime(2020, 06, 20, 20, 0, 0),
                                IsActive = true,
                            },
                            new Event
                            {
                                Title = "Bob's Food Event",
                                Description = "Bob's Food Event Description...",
                                PlaceName = "Foody Max",
                                CategoryId = foodsGuid,
                                StartDate = new DateTime(2020, 07, 02, 15, 0, 0),
                                EndDate = new DateTime(2020, 07, 02, 16, 30, 0),
                                IsActive = true,
                            }
                        }
                    },
                     new User
                    {
                        Username = "Mary",
                        Email = "mary@test.com",
                        Password = Hasher<User>.Hash(null, "123123"),
                        Gender = "F",
                        BirthDate = new DateTime(1985, 07, 17),
                        Events = new List<Event>
                        {
                            new Event
                            {
                                Title = "Mary's Music Event",
                                Description = "Mary's Music Event Description...",
                                PlaceName = "At My Home",
                                CategoryId = musicsGuid,
                                StartDate = new DateTime(2020, 06, 11, 19, 30, 0),
                                EndDate = new DateTime(2020, 06, 11, 21, 30, 0),
                                IsActive = true,
                                MaxNumberOfPeople = 20
                            },
                            new Event
                            {
                                Title = "Mary's Dance Event",
                                Description = "Mary's Dance Event Description...",
                                PlaceName = "Dance Club",
                                CategoryId = danceGuid,
                                StartDate = new DateTime(2020, 06, 18, 23, 0, 0),
                                EndDate = new DateTime(2020, 06, 19, 01, 0, 0),
                                Price = 50,
                                MaxAgeLimit = 40,
                                IsActive = true,
                            }
                        }
                    }

                };


                await context.Users.AddRangeAsync(users);
                await context.SaveChangesAsync();
            }

        }

    }
}
