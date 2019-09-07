import { TestBed, inject } from '@angular/core/testing';

import { AuthedHttpClientService } from './authed-http-client.service';

describe('AuthedHttpClientService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AuthedHttpClientService]
    });
  });

  it('should be created', inject([AuthedHttpClientService], (service: AuthedHttpClientService) => {
    expect(service).toBeTruthy();
  }));
});
