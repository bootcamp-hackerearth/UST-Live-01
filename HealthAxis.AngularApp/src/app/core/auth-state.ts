import { signal } from '@angular/core';

export const authState = signal({
  token: '',
  role: '',
  name: '',
  isLoggedIn: false
});
