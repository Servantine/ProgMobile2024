namespace uts.Pages;
using uts.Models;
using uts.Pages;

public partial class CoursesPage : ContentPage
{
    private readonly ApiService _apiService;
    public CoursesPage()
	{
        InitializeComponent();
        _apiService = new ApiService();
        LoadCourses();
    }
    private async void LoadCourses()
    {
        try
        {
            var Courses = await _apiService.GetCoursesAsync();
            CoursesCollectionView.ItemsSource = Courses;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load categories: {ex.Message}", "OK");
        }
    }
    private async void OnCategoryButtonClicked(object sender, EventArgs e)
    {

        await Navigation.PushAsync(new MainPage());
    }
    private async void OnAddCourseClicked(object sender, EventArgs e)
    {
        
        string courseName = await DisplayPromptAsync("Add Course", "Enter the course name:");
        if (string.IsNullOrWhiteSpace(courseName))
        {
            await DisplayAlert("Invalid Input", "Please enter a valid course name.", "OK");
            return;
        }

       
        string imageName = await DisplayPromptAsync("Add Course", "Enter the image name (optional):");

        
        string durationInput = await DisplayPromptAsync("Add Course", "Enter the course duration (in hours):");
        double? courseDuration = null;
        if (!string.IsNullOrWhiteSpace(durationInput) && double.TryParse(durationInput, out double parsedDuration))
        {
            courseDuration = parsedDuration;
        }

        
        string courseDescription = await DisplayPromptAsync("Add Course", "Enter the course description:");

        
        string categoryIdInput = await DisplayPromptAsync("Add Course", "Enter the category ID:");
        int categoryId = 0;
        if (!string.IsNullOrWhiteSpace(categoryIdInput) && int.TryParse(categoryIdInput, out int parsedCategoryId))
        {
            categoryId = parsedCategoryId;
        }
        else
        {
            await DisplayAlert("Invalid Input", "Please enter a valid category ID.", "OK");
            return;
        }

        
        var newCourse = new Courses
        {
            name = courseName,
            imageName = imageName,
            duration = courseDuration,
            description = courseDescription,
            categoryId = categoryId
        };

        
        bool success = await _apiService.PostCourseAsync(newCourse);
        if (success)
        {
            await DisplayAlert("Success", "Course added successfully!", "OK");
            LoadCourses();
        }
        else
        {
            await DisplayAlert("Error", "Failed to add the course.", "OK");
        }

    }
    private async void OnEditCoursesClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var course = button?.CommandParameter as Courses;
        if (course == null) return;

        
        string newName = await DisplayPromptAsync("Edit Course", "Enter the new course name:", initialValue: course.name);
        if (string.IsNullOrWhiteSpace(newName))
        {
            await DisplayAlert("Invalid Input", "Please enter a valid name.", "OK");
            return;
        }

       
        string newDescription = await DisplayPromptAsync("Edit Course", "Enter the new course description:", initialValue: course.description);
        if (string.IsNullOrWhiteSpace(newDescription))
        {
            await DisplayAlert("Invalid Input", "Please enter a valid description.", "OK");
            return;
        }

      
        string durationInput = await DisplayPromptAsync("Edit Course", "Enter the new course duration (in hours):", initialValue: course.duration?.ToString());
        double? newDuration = null;
        if (!string.IsNullOrWhiteSpace(durationInput) && double.TryParse(durationInput, out double parsedDuration))
        {
            newDuration = parsedDuration;
        }

     
        course.name = newName;
        course.description = newDescription;
        course.duration = newDuration;

        
        bool success = await _apiService.PutCourseAsync(course);
        if (success)
        {
            await DisplayAlert("Success", "Course updated successfully!", "OK");
            LoadCourses();
        }
        else
        {
            await DisplayAlert("Error", "Failed to update the course.", "OK");
        }
    }
    private async void OnDeleteCoursesClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var course = button?.CommandParameter as Courses;
        if (course == null) return;

        
        bool confirm = await DisplayAlert("Delete Course", $"Are you sure you want to delete course '{course.name}'?", "Yes", "No");
        if (!confirm) return;

        
        bool success = await _apiService.DeleteCourseAsync(course.courseId);
        if (success)
        {
            await DisplayAlert("Success", "Course deleted successfully!", "OK");
            LoadCourses();
        }
        else
        {
            await DisplayAlert("Error", "Failed to delete the course.", "OK");
        }
    }
    private async void OnSearchCourseByIdClicked(object sender, EventArgs e)
    {
        if (int.TryParse(CourseIdEntry.Text, out int courseId))
        {
            var course = await _apiService.GetCourseByIdAsync(courseId);
            if (course != null)
            {
                CourseSearchResultStack.IsVisible = true;
                CourseSearchResultLabel.Text = $"Course ID: {course.courseId}\n" +
                                               $"Name: {course.name}\n" +
                                               $"Image Name: {course.imageName}\n" +
                                               $"Duration: {course.duration} hours\n" +
                                               $"Description: {course.description}\n" +
                                               $"Category ID: {course.categoryId}";
            }
            else
            {
                await DisplayAlert("Not Found", "Course with the specified ID not found.", "OK");
                CourseSearchResultStack.IsVisible = false;
            }
        }
        else
        {
            await DisplayAlert("Invalid Input", "Please enter a valid numeric ID.", "OK");
        }
    }



}