import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  imports: [RouterLink],
  selector: 'app-jerseys',
  styleUrl: './jerseys.scss',
  templateUrl: './jerseys.html',
})
export class Jerseys {
  jerseys = [
    { id: 1, name: 'Jersey' },
    { id: 2, name: 'Jersey' },
    { id: 3, name: 'Jersey' },
    { id: 4, name: 'Jersey' },
    { id: 5, name: 'Jersey' }
  ]
}
