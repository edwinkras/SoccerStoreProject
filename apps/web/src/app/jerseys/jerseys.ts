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
    { id: 1, name: '1' },
    { id: 2, name: '2' },
    { id: 3, name: '3' },
    { id: 4, name: '4' },
    { id: 5, name: '5' }
  ]
}
