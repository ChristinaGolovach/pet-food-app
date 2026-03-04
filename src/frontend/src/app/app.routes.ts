import { Routes } from '@angular/router';

export const routes: Routes = [
    { path: '', redirectTo: 'analysis', pathMatch: 'full' },
    { path: 'analysis', loadComponent: () => import('./components/analysis-page/analysis-page.component').then(m => m.AnalysisPageComponent) },
    { path: '**', redirectTo: 'analysis' }
];
