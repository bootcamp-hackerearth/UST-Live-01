import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-pagination',
  imports: [],
  templateUrl: './pagination.html',
  styleUrls: ['./pagination.css']
})
export class Pagination {
  @Input() currentPage = 1;
  @Input() pageSize = 5;
  @Input() totalItems = 0;

  @Output() pageChanged = new EventEmitter<number>();

  get totalPages(): number {
    return Math.ceil(this.totalItems / this.pageSize) || 1;
  }

  get pages(): number[] {
    return Array.from(
      {
        length: this.totalPages
      },
      (_, index) => index + 1
    );
  }

  goToPage(page: number): void {
    if (page < 1 || page > this.totalPages || page === this.currentPage) {
      return;
    }

    this.pageChanged.emit(page);
  }

  goToPrevious(): void {
    this.goToPage(this.currentPage - 1);
  }

  goToNext(): void {
    this.goToPage(this.currentPage + 1);
  }
}