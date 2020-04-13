﻿namespace MyFitScope.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using MyFitScope.Data.Models;
    using MyFitScope.Data.Models.FitnessModels.Enums;
    using MyFitScope.Services.Data;
    using MyFitScope.Web.ViewModels.Exercises;

    public class ExercisesController : Controller
    {
        private readonly IExercisesService exercisesService;
        private readonly UserManager<ApplicationUser> userManager;

        public ExercisesController(IExercisesService exercisesService, UserManager<ApplicationUser> userManager)
        {
            this.exercisesService = exercisesService;
            this.userManager = userManager;
        }

        public IActionResult CreateExercise()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateExercise(CreateExerciseInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var creatorName = this.User.Identity.Name;

            await this.exercisesService.CreateExerciseAsync(input.Name, input.VideoUrl, input.MuscleGroup, input.Description, creatorName);

            return this.RedirectToAction("ExercisesListing", new { exerciseCategory = "Custom" });
        }

        public IActionResult ExercisesListing(string exerciseCategory)
        {
            var userName = this.User.Identity.Name;

            var model = new ExerciseListingViewModel
            {
                Exercises = this.exercisesService.GetExercisesByCategory(userName, exerciseCategory),
                ListedGroupType = exerciseCategory ?? "All",
            };

            return this.View(model);
        }

        public IActionResult Details(string id)
        {
            var model = this.exercisesService.GetExerciseById<DetailsExerciseViewModel>(id);

            return this.View(model);
        }

        public IActionResult GetMuscleGroups()
        {
            var result = Enum.GetNames(typeof(MuscleGroup))
                        .Select(ac => new ExerciseMuscleGroupOutputModel
                        {
                            GroupName = ac,
                        })
                        .ToList();

            return this.Ok(result);
        }

        public IActionResult GetExercisesByMuscleGroup(string muscleGroup)
        {
            var userName = this.User.Identity.Name;

            var exercises = this.exercisesService.GetExercisesByCategory(userName, muscleGroup);

            var result = exercises.Select(e => new ExerciseOutputModel
            {
                Id = e.Id,
                Name = e.Name,
            });

            return this.Ok(result);
        }

        public async Task<IActionResult> DeleteExercise(string exerciseId)
        {
            await this.exercisesService.DeleteExerciseAsync(exerciseId);

            return this.RedirectToAction("ExercisesListing");
        }

        public IActionResult EditExercise(string exerciseId)
        {
            var model = this.exercisesService.GetExerciseById<EditExerciseInputViewModel>(exerciseId);

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditExercise(EditExerciseInputViewModel input)
        {
            await this.exercisesService.EditExerciseAsync(input.Id, input.Name, input.VideoUrl, input.MuscleGroup, input.Description);

            return this.RedirectToAction(nameof(this.Details), new { id = input.Id });
        }
    }
}
