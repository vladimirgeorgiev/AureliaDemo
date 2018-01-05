import { autoinject, NewInstance } from "aurelia-framework";
import { MovieData } from "./movieData";
import { MovieItem } from "./movieData";
import { Router } from "aurelia-router";
import { BootstrapFormRenderer } from "./BookstrapFormRenderer";
import { ValidationRules, ValidationController, ValidationControllerFactory, Validator, validateTrigger } from 'aurelia-validation';


@autoinject
export class Edit {
   

    data: MovieData;
    router: Router;
    movie: MovieItem;
    validator: Validator;
    private controller: ValidationController;
    public canSave: Boolean;

    constructor(movieData: MovieData, router: Router, validator: Validator, controllerFactory: ValidationControllerFactory) {
        this.data = movieData;
        this.router = router;
        this.validator = validator;
        this.controller = controllerFactory.createForCurrentScope(validator);
        this.controller.validateTrigger = validateTrigger.changeOrBlur;
        this.controller.subscribe(event => this.validateWhole());

        
    }
   
    private validateWhole() {
        this.validator.validateObject(this.movie)
            .then(results => this.canSave = results.every(result => result.valid));
    }

    public setupValidation() {
        ValidationRules
            .ensure((m: MovieItem) => m.title).required().minLength(3).withMessage('Title must at least be 3 chars long.')
            .ensure((m: MovieItem) => m.releaseYear).required().minLength(3).withMessage('Description must at least be 3 chars long.')
            .on(this.movie);
    }  

    activate(params: any) {
        if (params.id) {
            this.data.getById(params.id).then(result => {
                this.movie = result;
                this.setupValidation();
            });
        } else {
            this.movie = new MovieItem();
            this.setupValidation();    
        }
    }

    save() {
        this.controller.validate().then(v => {
            if (v.valid) {
                this.data.save(this.movie).then(x => {
                    let url = this.router.generate("details", { id: x.id });
                    this.router.navigate(url);
                });
            }
        });
    }
}