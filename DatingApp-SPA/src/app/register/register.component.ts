import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

@Output() cencelRegister=new EventEmitter(); // step: 1 for child to parent
  model:any={};
  constructor(private authservice:AuthService) { }

  ngOnInit() {
  }
register(){
  this.authservice.register(this.model).subscribe(()=>{
    console.log('Register successful');
  },
  error=>{
console.log('faild register');
  })
}
cencel(){
  this.cencelRegister.emit(false); // step:2 for child to parent
  console.log('Cenceled');
}
}
