import { autoinject } from 'aurelia-framework';
import { HttpClient,json } from 'aurelia-fetch-client';

@autoinject
export class MovieData {
    
    http: HttpClient;
    baseUrl = 'api/SampleData/Movies';
    json: JSON;

    movies: MovieItem[]
    constructor(httpClient: HttpClient) {
        this.http = httpClient;

        this.http.configure(config => {
            config
                .useStandardConfiguration()
                .withDefaults({
                    //credentials: 'same-origin',
                    headers: {
                        'X-Requested-With': 'Fetch'
                    }
                });
        });
    }

    async getAll() {
        this.movies = await this.http.fetch(this.baseUrl).
            then(result => result.json() as Promise<MovieItem[]>);
        return this.movies;
    }

    async getById(id: number) {
        return await this.http.fetch(this.baseUrl + '/' + id).
            then(result => result.json() as Promise<MovieItem>);
    } 


    async save(movie: MovieItem) {
        return await this.http.fetch(this.baseUrl,
            {
                method: 'put',
                body: json(movie)
            }).then(result => result.json() as Promise<MovieItem>);
    }

}

export class MovieItem {
    title: string;
    releaseYear: number;
    id: number;
}