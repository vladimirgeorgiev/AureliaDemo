import { autoinject } from 'aurelia-framework';
import { MovieData } from './movieData';
import { MovieItem } from './movieData';


@autoinject
export class List {
  
    movieData: MovieData;
    movies: MovieItem[];
    

    constructor(movieData: MovieData) {
        this.movieData = movieData;
    }

   
    async activate() {
        this.movies = await this.movieData.getAll();
    }
}

