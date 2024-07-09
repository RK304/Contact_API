import { Component, OnInit } from '@angular/core';
import { Contact } from '../../Models/contact.model'; 
import { ContactService } from '../../services/contact/contact.service';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { ToastrModule, ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-contact-list',
  standalone: true,
  imports: [CommonModule,RouterModule,HttpClientModule,ToastrModule], 
  templateUrl: './contact-list.component.html',
  styleUrls: ['./contact-list.component.css']
})
export class ContactListComponent implements OnInit {
  contacts: Contact[] = [];

  constructor(private contactService: ContactService, private toastr: ToastrService) {}

  ngOnInit(): void {
    this.getContacts();
  }

  getContacts(): void {
    this.contactService.getContacts().subscribe(
      (data: Contact[]) => {
        this.contacts = data;
      }
    );
  }

  editContact(contact: Contact): void {
    console.log('Edit contact:', contact);
  }

  deleteContact(contactId: number): void {
    if (confirm('Are you sure you want to delete this contact?')) {
      this.contactService.deleteContact(contactId).subscribe(() => {
        // this.toastr.success('Contact Deleted successfully', 'Success');
        this.getContacts();
      });
    }
  }
}
