import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LandingComponent } from './pages/landing/landing.component';
import { LoginComponent } from './auth/login/login.component';
import { RegisterComponent } from './auth/register/register.component';
import { BookListComponent } from './pages/book/book-list/book-list.component';
import { BookAddComponent } from './pages/book/book-add/book-add.component';
import { BookEditComponent } from './pages/book/book-edit/book-edit.component';
import { QuoteListComponent } from './pages/quote/quote-list/quote-list.component';
import { QuoteAddComponent } from './pages/quote/quote-add/quote-add.component';
import { QuoteEditComponent } from './pages/quote/quote-edit/quote-edit.component';
import { AuthGuard } from './guards/auth-guard';

const routes: Routes = [
  { path: '', component: LandingComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'books', component: BookListComponent, canActivate: [AuthGuard] },
  { path: 'add-book', component: BookAddComponent, canActivate: [AuthGuard] },
  { path: 'edit-book/:id', component: BookEditComponent, canActivate: [AuthGuard] },
  { path: 'quotes', component: QuoteListComponent, canActivate: [AuthGuard] },
  { path: 'add-quote', component: QuoteAddComponent, canActivate: [AuthGuard] },
  { path: 'edit-quote/:id', component: QuoteEditComponent, canActivate: [AuthGuard] }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }