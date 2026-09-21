import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  imports: [RouterLink],
  selector: 'app-cleats',
  styleUrl: './cleats.scss',
  templateUrl: './cleats.html',
})
export class Cleats {
  cleats = [
    { id: 1, name: 'Cleats' },
    { id: 2, name: 'Cleats' },
    { id: 3, name: 'Cleats' },
    { id: 4, name: 'Cleats' },
    { id: 5, name: 'Cleats' }
  ]
}
