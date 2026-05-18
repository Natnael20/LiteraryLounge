import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { AppRoutingModule } from './app-routing-module';
import { App } from './app';
import { FooterComponent } from './components/footer/footer.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { LandingComponent } from './pages/landing/landing.component';
import { LoginComponent } from './auth/login/login.component';
import { RegisterComponent } from './auth/register/register.component';
import { BookListComponent } from './pages/book/book-list/book-list.component';
import { BookAddComponent } from './pages/book/book-add/book-add.component';
import { BookEditComponent } from './pages/book/book-edit/book-edit.component';
import { QuoteListComponent } from './pages/quote/quote-list/quote-list.component';
import { QuoteAddComponent } from './pages/quote/quote-add/quote-add.component';
import { QuoteEditComponent } from './pages/quote/quote-edit/quote-edit.component';
import { AuthInterceptor } from './interceptors/auth-interceptor';

@NgModule({
  declarations: [
    App,
    FooterComponent,
    NavbarComponent,
    LandingComponent,
    LoginComponent,
    RegisterComponent,
    BookListComponent,
    BookAddComponent,
    BookEditComponent,
    QuoteListComponent,
    QuoteAddComponent,
    QuoteEditComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    RouterModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
  ],
  bootstrap: [App]
})
export class AppModule { }