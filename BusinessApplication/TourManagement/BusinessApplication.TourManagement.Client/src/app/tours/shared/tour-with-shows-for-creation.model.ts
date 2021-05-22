import { TourForCreation } from "./tour-for-creation.model";
import { ShowForCreation } from "./show-for-creation.model";

export class TourWithShowsForCreation extends TourForCreation {
    shows: ShowForCreation[];
}
