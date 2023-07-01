import { Component, Input, OnInit } from "@angular/core";
import { FileUploader } from "ng2-file-upload";
import { take } from "rxjs";
import { Member } from "src/app/_models/member";
import { Photo } from "src/app/_models/photo";
import { User } from "src/app/_models/user";
import { AccountService } from "src/app/_services/account.service";
import { MembersService } from "src/app/_services/member.service";
import { environment } from "src/environments/environment";

@Component({
    selector: 'app-project-editor',
    templateUrl: './project-editor.component.html',
    styleUrls: ['./project-editor.component.css']
  })

  export class ProjectEditorComponent implements OnInit {
    @Input() member: Member | undefined;
    uploader: FileUploader | undefined;
    hasBaseDropZoneOver = false;
    baseUrl = environment.apiUrl;
    user: User | undefined;
  
    constructor(private accountService: AccountService, private memberService: MembersService) {
        this.accountService.currentUser$.pipe(take(1)).subscribe({
            next: user => {
                if(user) this.user = user
            }
        })
    }
  
    ngOnInit(): void{
       this.initializeUploader();

    }

    fileOverBase(e: any){
        this.hasBaseDropZoneOver = e;
    }



    deleteProject(projectId: number) {
        this.memberService.deleteProject(projectId).subscribe({
            next: () => {
                if (this.member) {
                    this.member.projects = this.member.projects.filter(x => x.id !== projectId);
                }
            }
        })
    }

    initializeUploader(){
        this.uploader = new FileUploader({
            url: this.baseUrl + 'users/add-project',
            authToken: 'Bearer ' + this.user?.token,
            isHTML5: true,
            removeAfterUpload: true,
            autoUpload: false,
            maxFileSize: 10 * 1024 * 1024
        });

        this.uploader.onAfterAddingFile = (file) => {
            file.withCredentials = false
        }

        this.uploader.onSuccessItem = (item, response, status, headers) => {
            if (response){
                const project = JSON.parse(response);
                project.fileName= item.file.name;

                if(this.member?.projects)
                {
                    this.member?.projects.push(project);    
                }
                else
                {
                    if(this.member != undefined)
                    {
                        this.member.projects = []; 
                        this.member.projects.push(project);    
                    }  
                }
            }
        }
    }


 
}
