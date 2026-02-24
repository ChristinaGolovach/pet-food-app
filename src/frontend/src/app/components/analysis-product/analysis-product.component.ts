import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AnalysisService } from '../../services/analysis.service';

@Component({
  selector: 'app-analysis-product',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './analysis-product.component.html',
  styleUrl: './analysis-product.component.scss'
})
export class AnalysisProductComponent {
  imagePreview = signal<string | null>(null);
  allergens: string = '';
  private analysisService = inject(AnalysisService);

  private selectedFile: File | null = null;

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
    this.analysisService.analyze(this.selectedFile!, this.allergens).subscribe(result => {
      console.log('Analysis result:', result);
    });
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
