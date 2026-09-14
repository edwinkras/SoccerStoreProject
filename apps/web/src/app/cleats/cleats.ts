import { Component } from '@angular/core';

@Component({
  imports: [],
  selector: 'app-cleats',
  styleUrl: './cleats.scss',
  templateUrl: './cleats.html',
})
export class Cleats {
  cleats = [
    { id: 1, name: '1' },
    { id: 2, name: '2' },
    { id: 3, name: '3' },
    { id: 4, name: '4' },
    { id: 5, name: '5' }
  ]
}
