import { Component, EventEmitter, inject, Output, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-analysis-product',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './analysis-product.component.html',
  styleUrl: './analysis-product.component.scss'
})
export class AnalysisProductComponent {
  @Output() analyzeSubmitEvent = new EventEmitter<{ file: File; allergens: string }>();

  private selectedFile: File | null = null;

  imagePreview = signal<string | null>(null);
  allergens: string = '';


  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files[0]) {
      this.previewFile(input.files[0]);
    }
  }

  removeImage(): void {
    this.selectedFile = null;
    this.imagePreview.set(null);
  }

  onSubmit(): void {
      this.analyzeSubmitEvent.emit({ file: this.selectedFile!, allergens: this.allergens });
  }

  private previewFile(file: File): void {
    this.selectedFile = file;
    const reader = new FileReader();
    reader.onload = () => {
      this.imagePreview.set(reader.result as string);
    };
    reader.readAsDataURL(file);
  }

}
