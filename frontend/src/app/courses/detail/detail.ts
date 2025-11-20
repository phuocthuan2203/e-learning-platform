import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CourseService } from '../../core/services/course.service';
import { CourseDetail } from '../../core/models/course.models';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-course-detail',
  standalone: false,
  templateUrl: './detail.html',
  styleUrl: './detail.scss',
})
export class CourseDetailComponent implements OnInit {
  course: CourseDetail | null = null;
  isLoading = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private courseService: CourseService,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    const courseId = this.route.snapshot.params['id'];
    if (courseId) {
      this.loadCourseDetails(+courseId);
    }
  }

  loadCourseDetails(courseId: number): void {
    this.isLoading = true;

    this.courseService.getDetails(courseId).subscribe({
      next: (course: CourseDetail) => {
        this.course = course;
        this.isLoading = false;
      },
      error: (error) => {
        this.isLoading = false;
        const msg = error.error?.message || 'Failed to load course details.';
        this.snackBar.open(msg, 'Close', { duration: 5000, panelClass: 'error-snackbar' });
        
        // Redirect back to catalog if course not found
        if (error.status === 404) {
          setTimeout(() => this.router.navigate(['/courses']), 2000);
        }
      },
    });
  }

  goBack(): void {
    this.router.navigate(['/courses']);
  }

  getTotalLessons(): number {
    if (!this.course) return 0;
    return this.course.modules.reduce((total, module) => total + module.lessons.length, 0);
  }

  formatDate(dateString: string): string {
    const date = new Date(dateString);
    return date.toLocaleDateString('en-US', { year: 'numeric', month: 'long', day: 'numeric' });
  }
}
