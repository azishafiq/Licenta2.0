import { Component, OnInit } from '@angular/core';
import { Member } from '../_models/member';
import { MembersService } from '../_services/member.service';
import { Pagination } from '../_models/pagination';

@Component({
    selector: 'app-lists',
    templateUrl: './lists.component.html',
    styleUrls: ['./lists.component.css']
  })

  export class ListsComponent implements OnInit {
    members: Member[] | undefined;
    predicate ='added';
    pageNumber = 1;
    pageSize = 6;
    pagination: Pagination | undefined;
    
  
    constructor(private memberService: MembersService) {}
  
    ngOnInit(): void{
      
    }
    
    loadAdds() {
      this.memberService.getAdds(this.predicate, this.pageNumber, this.pageSize).subscribe({
        next: response => {
          this.members = response.result;
          this.pagination = response.pagination;
        }
      })
    }

    pageChanged(event : any) {
      if(this.pageNumber !== event.page) {
        this.pageNumber = event.page;
        this.loadAdds();
      }
     
    }
  
  
  }
  