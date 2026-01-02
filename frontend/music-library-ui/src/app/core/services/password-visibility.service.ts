import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class PasswordVisibilityService {

  /** 
  * Toggle password visibility for a specific input field
  * @param inputElement - The HTML input element to be modified
  * @param currentState - The current visibility state (true = visible, false = hidden)
  * @returns The new visibility state
  */
  togglePasswordVisibility(inputElement: HTMLInputElement | null, currentState: boolean): boolean {
    if (!inputElement) {
      return currentState;
    }

    const newState = !currentState;
    inputElement.type = newState ? 'text' : 'password';
    return newState;
  }

  /**
   * Toggle password visibility using a CSS selector
   * Useful when you know the formControlName or another selector
   * @param selector - CSS selector to find the input (e.g. input[formControlName="password"])
   * @param currentState - The current visibility state
   * @returns The new visibility state
   */
  togglePasswordVisibilityBySelector(selector: string, currentState: boolean): boolean {
    const passwordInput = document.querySelector(selector) as HTMLInputElement;
    return this.togglePasswordVisibility(passwordInput, currentState);
  }

  /**
   * Helper method to retrieve the input element via formControlName
   * @param formControlName
   * @returns The HTML input element or null
   */
  getPasswordInput(formControlName: string): HTMLInputElement | null {
    return document.querySelector(`input[formControlName="${formControlName}"]`) as HTMLInputElement;
  }
}
