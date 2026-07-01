import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-pagination',
  imports: [],
  templateUrl: './pagination.html',
  styleUrl: './pagination.css',
})
export class Pagination {
  @Input({ required: true }) totalItems = 0;
  @Input() pageSize = 5;
  @Input() currentPage = 1;

  @Output() pageChange = new EventEmitter<number>();

  get totalPages(): number {
    return Math.max(Math.ceil(this.totalItems / this.pageSize), 1);
  }

  get startItem(): number {
    if (this.totalItems === 0) {
      return 0;
    }

    return (this.currentPage - 1) * this.pageSize + 1;
  }

  get endItem(): number {
    return Math.min(this.currentPage * this.pageSize, this.totalItems);
  }

  goToPreviousPage(): void {
    if (this.currentPage <= 1) {
      return;
    }

    this.pageChange.emit(this.currentPage - 1);
  }

  goToNextPage(): void {
    if (this.currentPage >= this.totalPages) {
      return;
    }

    this.pageChange.emit(this.currentPage + 1);
  }
}
