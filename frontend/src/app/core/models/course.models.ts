export interface CourseSummary {
  courseId: number;
  title: string;
  description: string;
  imageUrl?: string;
  instructorName: string;
  createdDate: string;
}

export interface Lesson {
  lessonId: number;
  title: string;
  textContent?: string;
  externalVideoUrl?: string;
  order: number;
}

export interface Module {
  moduleId: number;
  title: string;
  order: number;
  lessons: Lesson[];
}

export interface CourseDetail {
  courseId: number;
  title: string;
  description: string;
  imageUrl?: string;
  instructorName: string;
  createdDate: string;
  modules: Module[];
}

export interface PagedResult<T> {
  items: T[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}
