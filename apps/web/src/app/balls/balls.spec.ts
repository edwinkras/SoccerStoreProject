import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Balls } from './balls';

describe('Balls', () => {
  let component: Balls;
  let fixture: ComponentFixture<Balls>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Balls],
    }).compileComponents();

    fixture = TestBed.createComponent(Balls);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
