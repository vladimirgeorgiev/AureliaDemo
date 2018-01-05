import { MovieData } from './movieData';
import { MovieItem } from './movieData';
import { autoinject  } from 'aurelia-framework';
import { Router  } from 'aurelia-router';

@autoinject 
export class Details {

    data: MovieData;
    movie: MovieItem;

    constructor(movieData:MovieData) {
        this.data = movieData;
    }

    async activate(params:any) {
        this.movie = await this.data.getById(params.id);
    }

}