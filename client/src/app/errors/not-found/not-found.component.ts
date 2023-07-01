import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'app-lists',
    templateUrl: './not-found.component.html',
    styleUrls: ['./not-found.component.css']
  })

  export class NotFoundComponent implements OnInit {

    
  
    constructor(private http: HttpClient) {}
  
    ngOnInit(): void{
      
    }
  }