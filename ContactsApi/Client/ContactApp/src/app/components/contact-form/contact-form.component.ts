import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Contact } from '../../Models/contact.model'; 
import { ContactService } from '../../services/contact/contact.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { ToastrModule, ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-contact-form',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule,ToastrModule],
  templateUrl: './contact-form.component.html',
  styleUrls: ['./contact-form.component.css']
})
export class ContactFormComponent implements OnInit {
  contactId: number | null = null;
  newContact: Contact = {
    firstName: '',
    lastName: '',
    email: '',
    phoneNumber: '',
    address: '',
    city: '',
    state: '',
    country: '',
    postalCode: ''
  };

  constructor(
    private contactService: ContactService,
    private route: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.contactId = +idParam;
      this.fetchContactDetails(this.contactId);
    }
  }

  private fetchContactDetails(id: number): void {
    this.contactService.getContactById(id).subscribe((contact: Contact) => {
      this.newContact = contact;
    });
  }

  onSubmit(): void {
    if (this.contactId) {
      this.contactService.updateContact(this.contactId, this.newContact).subscribe(() => {
        // this.toastr.success('Contact Updated successfully', 'Success');
        this.router.navigate(['/contacts']);
      });
    } else {
      this.contactService.addContact(this.newContact).subscribe(() => {
        // this.toastr.success('Contact added successfully', 'Success');
        this.router.navigate(['/contacts']);
      });
    }
  }
}
