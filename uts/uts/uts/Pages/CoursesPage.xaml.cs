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
    private async void OnCategoryButtonClicked(object sender, EventArgs e)
    {

        await Navigation.PushAsync(new MainPage());
    }
    
    private async void OnUsersButtonClicked(object sender, EventArgs e)
    {

        await Navigation.PushAsync(new InstructorPage());
    }
    private async void OnAddCourseClicked(object sender, EventArgs e)
    {
        try
        {
            // Prompt for course name
            string courseName = await DisplayPromptAsync("Add Course", "Enter the course name:");
            if (string.IsNullOrWhiteSpace(courseName))
            {
                await DisplayAlert("Invalid Input", "Course name cannot be empty.", "OK");
                return;
            }

            // Prompt for image name
            string imageName = await DisplayPromptAsync("Add Course", "Enter the image name (optional):");

            // Prompt for duration
            string durationInput = await DisplayPromptAsync("Add Course", "Enter the course duration (in hours):");
            double? courseDuration = null;
            if (!string.IsNullOrWhiteSpace(durationInput))
            {
                if (double.TryParse(durationInput, out double parsedDuration) && parsedDuration > 0)
                {
                    courseDuration = parsedDuration;
                }
                else
                {
                    await DisplayAlert("Invalid Input", "Please enter a valid positive number for course duration.", "OK");
                    return;
                }
            }

            // Prompt for course description
            string courseDescription = await DisplayPromptAsync("Add Course", "Enter the course description:");

            // Prompt for category ID
            string categoryIdInput = await DisplayPromptAsync("Add Course", "Enter the category ID:");
            int categoryId = 0;
            if (!string.IsNullOrWhiteSpace(categoryIdInput))
            {
                if (int.TryParse(categoryIdInput, out int parsedCategoryId) && parsedCategoryId > 0)
                {
                    categoryId = parsedCategoryId;
                }
                else
                {
                    await DisplayAlert("Invalid Input", "Please enter a valid positive integer for category ID.", "OK");
                    return;
                }
            }
            else
            {
                await DisplayAlert("Invalid Input", "Category ID cannot be empty.", "OK");
                return;
            }

            // Create an anonymous object for the API payload
            var newCourse = new Courses
            {
                courseId = 0,
                name = courseName,
                imageName = imageName,
                duration = courseDuration,
                description = courseDescription,
                Category = new Categories { categoryId = categoryId } // Map categoryId here
            };

            // Call API to add course
            bool success = await _apiService.PostCourseAsync(newCourse);
            if (success)
            {
                await DisplayAlert("Success", "Course added successfully!", "OK");
                LoadCourses(); // Reload the courses
            }
            else
            {
                await DisplayAlert("Error", "Failed to add the course. Please try again.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An unexpected error occurred: {ex.Message}", "OK");
        }
    }


    private async void OnEditCoursesClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var course = button?.CommandParameter as Courses;
        if (course == null) return;

        // Mengedit nama course
        string courseName = await DisplayPromptAsync("Edit Course", "Enter the course name:", initialValue: course.name);
        if (string.IsNullOrWhiteSpace(courseName))
        {
            await DisplayAlert("Invalid Input", "Please enter a valid course name.", "OK");
            return;
        }

        // Mengedit nama gambar
        string imageName = await DisplayPromptAsync("Edit Course", "Enter the image name (optional):", initialValue: course.imageName);

        // Mengedit durasi course
        string durationInput = await DisplayPromptAsync("Edit Course", "Enter the course duration (in hours):", initialValue: course.duration?.ToString());
        double? courseDuration = null;
        if (!string.IsNullOrWhiteSpace(durationInput) && double.TryParse(durationInput, out double parsedDuration))
        {
            courseDuration = parsedDuration;
        }

        // Mengedit deskripsi course
        string courseDescription = await DisplayPromptAsync("Edit Course", "Enter the course description:", initialValue: course.description);

        // Daftar kategori yang tersedia (untuk simulasi, bisa diubah dengan data nyata)
        var categories = await _apiService.GetCategoriesAsync(); // Mendapatkan daftar kategori
        if (categories == null || !categories.Any())
        {
            await DisplayAlert("Error", "No categories available. Please add categories first.", "OK");
            return;
        }

        // Menampilkan daftar kategori ke user
        string categoryList = string.Join("\n", categories.Select(c => $"{c.categoryId}: {c.name}"));
        await DisplayAlert("Available Categories", $"Here are the available categories:\n\n{categoryList}", "OK");

        // Meminta user untuk memilih kategori
        string categoryIdInput = await DisplayPromptAsync("Edit Course", "Enter the category ID:", initialValue: course.Category?.categoryId.ToString());
        if (!int.TryParse(categoryIdInput, out int selectedCategoryId) || !categories.Any(c => c.categoryId == selectedCategoryId))
        {
            await DisplayAlert("Invalid Input", "Please enter a valid category ID.", "OK");
            return;
        }

        // Mengupdate data course tanpa mengubah properti Category
        course.name = courseName;
        course.imageName = imageName;
        course.duration = courseDuration;
        course.description = courseDescription;

        // Kirimkan hanya categoryId bersama data lainnya ke server
        bool success = await _apiService.PutCourseWithCategoryIdAsync(course, selectedCategoryId);
        if (success)
        {
            await DisplayAlert("Success", "Course updated successfully!", "OK");
            LoadCourses(); // Memuat ulang daftar course
        }
        else
        {
            await DisplayAlert("Error", "Failed to update the course. Please check your internet connection or try again later.", "OK");
            Console.WriteLine("Failed to update course: " + course.courseId);
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
    private async void OnSearchCourseByNameClicked(object sender, EventArgs e)
    {
        string courseName = CourseNameEntry.Text;

        if (string.IsNullOrWhiteSpace(courseName))
        {
            await DisplayAlert("Invalid Input", "Please enter a valid course name.", "OK");
            return;
        }

        var course = await _apiService.GetCourseByNameAsync(courseName);
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
            await DisplayAlert("Not Found", $"Course with the name '{courseName}' not found.", "OK");
            CourseSearchResultStack.IsVisible = false;
        }
    }
    private async void OnEnrollInstructorClicked(object sender, EventArgs e)
    {
        try
        {
            // Ambil courseId dari CommandParameter
            var button = sender as Button;
            if (button?.CommandParameter is not int courseId)
            {
                await DisplayAlert("Error", "Failed to retrieve course information.", "OK");
                return;
            }

            // Ambil daftar instructors
            var instructors = await LoadInstructorsAsync();
            if (instructors == null || instructors.Count == 0)
            {
                await DisplayAlert("Error", "No instructors available to enroll.", "OK");
                return;
            }

            // Tampilkan dropdown untuk memilih instructor
            var instructorNames = instructors.Select(i => i.userName).ToArray();
            string selectedInstructorName = await DisplayActionSheet("Select an Instructor", "Cancel", null, instructorNames);

            // Batalkan jika pengguna tidak memilih
            if (selectedInstructorName == "Cancel" || string.IsNullOrWhiteSpace(selectedInstructorName))
            {
                return;
            }

            // Cari instructorId berdasarkan nama yang dipilih
            var selectedInstructor = instructors.FirstOrDefault(i => i.userName == selectedInstructorName);
            if (selectedInstructor == null)
            {
                await DisplayAlert("Error", "Invalid instructor selected.", "OK");
                return;
            }

            // Membentuk payload untuk API
            var enrollmentPayload = new
            {
                instructorId = selectedInstructor.instructorId,
                courseId = courseId
            };

            // Kirim data ke API
            var isEnrolled = await _apiService.PostEnrollmentAsync(enrollmentPayload);
            if (isEnrolled)
            {
                await DisplayAlert("Success", "Instructor enrolled successfully!", "OK");
            }
            else
            {
                await DisplayAlert("Error", "Failed to enroll the instructor.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }




}