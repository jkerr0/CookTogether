﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CookTogether.Models.Api
{
    public class MealApiService
    {
        private static readonly string API_KEY = "1"; //for development purposes  
        private static readonly string URL_PREFIX = $"https://www.themealdb.com/api/json/v1/{API_KEY}/";

        // c - for categories, i - for ingredients, a - for areas
        private static readonly string LIST_URL_FORMAT = "list.php?{0}=list"; 
        private static readonly string AREAS_URL = URL_PREFIX + String.Format(LIST_URL_FORMAT, "a");
        private static readonly string INGREDIENTS_URL = URL_PREFIX + String.Format(LIST_URL_FORMAT, "i");

        private static readonly string CATEGORIES_DETAILS_URL = URL_PREFIX + "categories.php";

        private static readonly string MEALS_BY_STARTS_WITH_URL_FORMAT = URL_PREFIX + "search.php?f={0}"; //STARTING STRING

        private static readonly string INGREDIENT_THUMBNAIL_URL_FORMAT = "https://www.themealdb.com/images/ingredients/{0}.png"; //INGREDIENT_NAME

        private readonly HttpClient httpClient;

        public MealApiService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<Category[]> getCategories()
        {
            Category[] categories = Array.Empty<Category>();

            try
            {
                CategoriesResponse response = await httpClient.GetFromJsonAsync<CategoriesResponse>(CATEGORIES_DETAILS_URL);
                if (response.Categories != null)
                {
                    categories = response.Categories;
                }
            }
            catch { }
            
            return categories; 
        }

        public async Task<string> getCategoriesListAsString()
        {
            string response;
            try
            {
                response = await httpClient.GetStringAsync(CATEGORIES_DETAILS_URL);
            }
            catch(HttpRequestException e)
            {
                response = e.Message;
            }
            return response;
        }

        public async Task<Area[]> getAreas()
        {
            Area[] areas = Array.Empty<Area>();
            try
            {
                AreasResponse response = await httpClient.GetFromJsonAsync<AreasResponse>(AREAS_URL);
                if (response.Areas != null)
                {
                    areas = response.Areas;
                }
            }
            catch(HttpRequestException e) { }
            return areas;
        }

        public async Task<Ingredient[]> getIngredients()
        {
            Ingredient[] ingredients = Array.Empty<Ingredient>();
            try
            {
                IngredientsResponse response = await httpClient.GetFromJsonAsync<IngredientsResponse>(INGREDIENTS_URL);
                if (response.Ingredients != null)
                {
                    ingredients = response.Ingredients;
                    foreach(var ingredient in ingredients)
                    {
                        ingredient.ThumbnailUrl = new Uri(Uri.EscapeUriString(String.Format(INGREDIENT_THUMBNAIL_URL_FORMAT, ingredient.Name)));
                    }
                }
            }
            catch(HttpRequestException e) { }
            return ingredients;
        }

        public async Task<Meal[]> getMealsStartingWith(string starting)
        {
            Meal[] meals = Array.Empty<Meal>();
            string mealsUrl = String.Format(MEALS_BY_STARTS_WITH_URL_FORMAT, starting);
            try
            {
                MealsResponse response = await httpClient.GetFromJsonAsync<MealsResponse>(mealsUrl);
                if (response.Meals != null)
                {
                    meals = response.Meals;
                }
            }
            catch(HttpRequestException e) { }
            return meals;
        }
    }
}