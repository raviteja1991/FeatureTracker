import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FeatureService } from '../feature.service';
import { Feature } from '../FeatureModels/feature.model';

@Component({
  selector: 'app-feature-detail',
  templateUrl: './feature-detail.component.html',
  styleUrls: ['./feature-detail.component.css']
})
export class FeatureDetailComponent implements OnInit {
  feature: Feature | undefined;

  constructor(private route: ActivatedRoute, private featureService: FeatureService) { }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.featureService.getFeature(+id).subscribe(
        data => this.feature = data,
        error => console.error('Error Fetching feature', error)
      );
    } else {
      console.error('Feature ID is not available');
    }
  }
}
