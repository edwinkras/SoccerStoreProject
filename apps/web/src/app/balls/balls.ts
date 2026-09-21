import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  imports: [RouterLink],
  selector: 'app-balls',
  styleUrl: './balls.scss',
  templateUrl: './balls.html',
})
export class Balls {
  balls = [
    { id: 1, name: 'Ball' },
    { id: 2, name: 'Ball' },
    { id: 3, name: 'Ball' },
    { id: 4, name: 'Ball' },
    { id: 5, name: 'Ball' }
  ]
}
