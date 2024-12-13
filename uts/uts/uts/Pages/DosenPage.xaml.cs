using uts.Models;

namespace uts.Pages;

public partial class DosenPage : ContentPage

{
    private readonly ApiService _apiService;
    public DosenPage()
	{
        InitializeComponent();
        _apiService = new ApiService();
        LoadCourses();
    }
    private async Task<List<Instructors>> LoadInstructorsAsync()
    {
        try
        {
            var instructors = await _apiService.GetInstructorsAsync();
            return instructors ?? new List<Instructors>();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load instructors: {ex.Message}", "OK");
            return new List<Instructors>();
        }
    }
    private async void LoadCourses()
    {
        try
        {
            var courses = await _apiService.GetCoursesAsync();
            CoursesCollectionView.ItemsSource = courses;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load courses: {ex.Message}", "OK");
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
                                               $"Category ID: {(course.Category?.categoryId.ToString() ?? "N/A")}";
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