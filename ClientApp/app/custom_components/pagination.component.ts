import { Component, Input, Output, OnInit, EventEmitter, OnChanges, SimpleChanges } from "@angular/core"; 

@Component({
    selector: 'pagination', 
    template: 
` 
<nav>
  <ul class="pagination">
    <li> <a (click)="onPrevPageClick()"> << </a> </li> 
    <li *ngFor="let pageNo of pages"> <a (click)="onPageClick(pageNo)" [class.active]="currentPage==pageNo"> {{pageNo}} </a> </li> 
    <li> <a (click)="onNextPageClick()"> >> </a> </li>
  </ul> 
</nav>
<h6> Debug, pagination component: totalCount={{totalCount}}, pageSize={{pageSize}} </h6>
`   , 
    styles: [ ".active { background-color: steelblue; color: white} " ]  
})

export class PaginationComponent implements OnInit, OnChanges { 

    @Input('page-size') pageSize: number; 
    @Input('total-count') totalCount: number;
    @Output('page-changed') pageChangeEvent = new EventEmitter();
    currentPage: number = 1;
    pageCount: number; 
    pages: number[] = [];


    ngOnInit(): void {
        console.log('PaginationComponent.ngOnInit() !') ;
        if (this.pageSize == 0)
            this.pageSize = 10; 
    }

    ngOnChanges(changes: SimpleChanges): void {
        console.log("ngOnChanges() executed !"); 
        console.log("Imported: pageSize=" + this.pageSize + ", totalCount=" + this.totalCount);
        this.pageSize = this.pageSize ? this.pageSize : (this.totalCount ? this.totalCount : 10); 
        this.pageCount = Math.floor(this.totalCount / this.pageSize) + (this.totalCount % this.pageSize > 0 ? 1 : 0);
        this.currentPage = 1;
        console.log('Calculated: pageCount=' + this.pageCount); 
        this.pages = [];
        for (var i = 1; i <= this.pageCount; i++) {
            this.pages.push(i);
        }; 
    }

    onPageClick(pageNo: number) { 
        console.log("onPageClick, pageNo=" + pageNo);
        this.currentPage = pageNo;
        this.pageChangeEvent.emit(this.currentPage); // passing a value from component to the event subscriber
    }

    onPrevPageClick() {
        if (this.currentPage > 1) {
            this.currentPage -= 1;
            this.pageChangeEvent.emit(this.currentPage);
        } 
    }

    onNextPageClick() {
        if (this.currentPage < this.totalCount) {
            this.currentPage -= 1;
            this.pageChangeEvent.emit(this.currentPage);
        }
    }

}