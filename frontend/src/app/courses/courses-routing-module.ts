import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CatalogComponent } from './catalog/catalog';
import { CourseDetailComponent } from './detail/detail';

const routes: Routes = [
  { path: '', component: CatalogComponent },
  { path: ':id', component: CourseDetailComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CoursesRoutingModule { }
