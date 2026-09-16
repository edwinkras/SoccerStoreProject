import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Jerseys } from './jerseys';

describe('Jerseys', () => {
  let component: Jerseys;
  let fixture: ComponentFixture<Jerseys>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Jerseys],
    }).compileComponents();

    fixture = TestBed.createComponent(Jerseys);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
