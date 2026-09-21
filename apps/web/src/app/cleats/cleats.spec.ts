import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Cleats } from './cleats';

describe('Cleats', () => {
  let component: Cleats;
  let fixture: ComponentFixture<Cleats>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Cleats],
    }).compileComponents();

    fixture = TestBed.createComponent(Cleats);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
